# Context Map — Интернет-магазин (E-Commerce)

> **Принцип:** 1 ограниченный контекст = 1 агрегат
> **Методология:** EventStorming → Subdomains → Bounded Contexts → Aggregates
> **Дата:** 2026-03-14

---

## Субдомены и ограниченные контексты

| Субдомен | Тип | Ограниченные контексты (= агрегаты) |
|----------|-----|--------------------------------------|
| **Catalog & Inventory** | Core | `Product`, `Category`, `StockItem` |
| **Identity & Access** | Generic | `Customer`, `Session` |
| **Shopping** | Core | `Cart`, `Order` |
| **Payment & Billing** | Core | `Payment`, `Invoice` |
| **Fulfillment** | Supporting | `Shipment`, `ReturnRequest` |
| **Reviews & Ratings** | Supporting | `Review`, `AggregateRating` |
| **Notifications & Marketing** | Supporting | `Notification`, `Campaign`, `Promotion`, `DiscountCode` |

**Итого:** 7 субдоменов, 16 ограниченных контекстов.

---

## Карта взаимосвязей

```
┌─────────────────────────────── Subdomain: Identity & Access (Generic) ────┐
│  ┌────────────┐    ┌───────────┐                                          │
│  │  Customer   │───▶│  Session   │                                         │
│  └──────┬─────┘    └───────────┘                                          │
└─────────┼─────────────────────────────────────────────────────────────────┘
          │ CustomerRegistered, CustomerLoggedIn
          ▼
┌─────────────────────────────── Subdomain: Catalog & Inventory (Core) ─────┐
│  ┌────────────┐    ┌────────────┐    ┌─────────────┐                      │
│  │  Product    │───▶│  Category  │    │  StockItem   │                     │
│  └──────┬─────┘    └────────────┘    └──────┬──────┘                      │
└─────────┼───────────────────────────────────┼─────────────────────────────┘
          │ ProductPublished                  │ StockReserved/Failed
          │                                   │
          ▼                                   ▼
┌─────────────────────────────── Subdomain: Shopping (Core) ────────────────┐
│  ┌────────────┐ ─PlaceOrder─▶ ┌────────────┐                             │
│  │   Cart      │               │   Order     │                            │
│  └────────────┘               └──────┬─────┘                             │
└──────────────────────────────────────┼────────────────────────────────────┘
                                       │ OrderPlaced, OrderCancelled
                    ┌──────────────────┼──────────────────┐
                    ▼                  ▼                   ▼
┌──── Subdomain: Payment (Core) ──┐  ┌── Subdomain: Fulfillment (Supp.) ──┐
│ ┌──────────┐   ┌──────────┐     │  │ ┌──────────┐  ┌───────────────┐    │
│ │ Payment  │──▶│ Invoice  │     │  │ │ Shipment │  │ ReturnRequest │    │
│ └────┬─────┘   └──────────┘     │  │ └────┬─────┘  └───────────────┘    │
└──────┼──────────────────────────┘  └──────┼─────────────────────────────┘
       │ PaymentCompleted                   │ ShipmentDelivered
       │ RefundIssued                       │ ReturnCompleted
       ▼                                    ▼
┌─────────────────────── Subdomain: Reviews & Ratings (Supporting) ────────┐
│  ┌────────────┐    ┌──────────────────┐                                  │
│  │  Review     │──▶│ AggregateRating  │                                  │
│  └────────────┘    └──────────────────┘                                  │
└──────────────────────────────────────────────────────────────────────────┘

┌─────────────────── Subdomain: Notifications & Marketing (Supporting) ────┐
│  ┌──────────────┐  ┌──────────┐  ┌───────────┐  ┌──────────────┐        │
│  │ Notification  │  │ Campaign │  │ Promotion │  │ DiscountCode │        │
│  └──────────────┘  └──────────┘  └───────────┘  └──────────────┘        │
│  ◀── слушает события из ВСЕХ остальных субдоменов                        │
└──────────────────────────────────────────────────────────────────────────┘
```

---

## Паттерны интеграции между ограниченными контекстами

| Upstream (BC) | Downstream (BC) | Паттерн | Канал |
|---------------|-----------------|---------|-------|
| Customer | Cart, Order | Customer-Supplier | Sync (API) |
| Session | Customer | Shared Kernel | In-process |
| Product | Cart, StockItem | Published Language | Sync (API) |
| Category | Product | Conformist | Sync (API) |
| StockItem | Order | Customer-Supplier | Async (Events) |
| Cart | Order | Customer-Supplier | Async (Events) |
| Order | Payment | Customer-Supplier | Async (Events) |
| Order | Shipment | Customer-Supplier | Async (Events) |
| Payment | Invoice | Customer-Supplier | Async (Events) |
| Payment | Order | Published Language | Async (Events) |
| Shipment | Order | Published Language | Async (Events) |
| Shipment | ReturnRequest | Conformist | Async (Events) |
| ReturnRequest | StockItem | Published Language | Async (Events) |
| ReturnRequest | Payment | Published Language | Async (Events) |
| Review | AggregateRating | Customer-Supplier | Async (Events) |
| Shipment | Review | Published Language | Async (Events) |
| Order | Notification | Published Language | Async (Events) |
| Payment | Notification | Published Language | Async (Events) |
| Shipment | Notification | Published Language | Async (Events) |
| Campaign | Notification | Customer-Supplier | Async (Commands) |
| Promotion | DiscountCode | Conformist | Sync (API) |
| DiscountCode | Cart | Published Language | Sync (API) |

---

## Поток ключевых событий

```
CustomerRegistered
  └▶ ItemAddedToCart
       └▶ OrderPlaced
            ├▶ StockReserved ──▶ PaymentInitiated ──▶ PaymentCompleted
            │                                              ├▶ InvoiceGenerated
            │                                              └▶ ShipmentCreated
            │                                                    └▶ ShipmentDelivered
            │                                                          ├▶ ReviewRequested
            │                                                          └▶ ReturnRequest (optional)
            │                                                                └▶ ReturnCompleted
            │                                                                      ├▶ RefundIssued
            │                                                                      └▶ StockReplenished
            └▶ NotificationSent (на каждом этапе)
```
