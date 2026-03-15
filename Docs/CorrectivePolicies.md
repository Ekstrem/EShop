# Corrective Policies — TODO

This document tracks corrective policies from the Aggregate Design Canvases that are **not yet implemented** in code. These are cross-aggregate sagas, process managers, and background reconciliation jobs.

---

## Catalog & Inventory

### Product
- [ ] **Orphaned variant cleanup** — фоновый процесс удаления вариантов без SKU (ежедневный sweep)
- [ ] **Price sync with Promotion** — при истечении Promotion, откатить цену к базовой

### Category
- [ ] **Cascade depth update** — при перемещении категории обновить глубину у всех потомков
- [ ] **Reparent orphaned products** — при деактивации категории переместить товары в "Uncategorized"

### StockItem
- [ ] **Reservation expiration** — TTL 15 минут, автоматический release просроченных резервов (Saga: StockItem ↔ Cart/Order)
- [ ] **Low stock notification** — при LowStockDetected → Notification context
- [ ] **Automatic reorder** — при OutOfStock → запрос на пополнение (если настроен supplier)

---

## Identity & Access

### Customer
- [ ] **GDPR data export** — по запросу собрать данные из всех контекстов (Saga)
- [ ] **GDPR data deletion** — каскадное удаление/анонимизация (Saga: Customer → Order, Review, Cart, Session, Notification)

### Session
- [ ] **Session cleanup** — фоновый sweep expired sessions (cron job)
- [ ] **Suspicious activity detection** — при >3 failed logins за 5 минут → notify + temporary lock

---

## Shopping

### Cart
- [ ] **Cart abandonment** — Auto-abandon после 30 дней неактивности (cron job)
- [ ] **Stock validation on checkout** — Saga: Cart → StockItem (проверка availability перед OrderPlaced)

### Order
- [ ] **Payment timeout** — если PaymentCompleted не пришёл в течение 30 минут → auto-cancel (Saga: Order ↔ Payment)
- [ ] **Delivery confirmation timeout** — если ShipmentDelivered не пришёл за 14 дней после Shipped → escalation
- [ ] **Order → Shipment orchestration** — Paid → автоматическое создание Shipment (Saga)

---

## Payment & Billing

### Payment
- [ ] **Webhook deduplication** — дедупликация по providerTransactionId
- [ ] **Refund failure escalation** — если провайдер не подтвердил refund → alert admin для ручного возврата
- [ ] **Settlement reconciliation** — ежедневная сверка с провайдером (cron job)

### Invoice
- [ ] **Credit note for refund** — при RefundIssued → автоматическая генерация Credit Note (Event consumer: Payment → Invoice)
- [ ] **Auto-send invoice** — при InvoiceGenerated → автоматическая отправка на email (Event consumer: Invoice → Notification)

---

## Fulfillment

### Shipment
- [ ] **Carrier webhook polling** — если carrier не шлёт webhooks, polling каждые 4 часа
- [ ] **Stuck shipment detection** — если статус не менялся >7 дней → alert
- [ ] **Return label generation** — при ReturnApproved → запрос label у carrier API (Saga: ReturnRequest → Shipment)

### ReturnRequest
- [ ] **Return period enforcement** — при RequestReturn проверять 14-day EU window (реализовано в Domain, но нужен cron для auto-reject expired)
- [ ] **Refund orchestration** — при ReturnCompleted → Payment.RequestRefund (Saga: ReturnRequest → Payment)
- [ ] **Restocking** — при ReturnCompleted → StockItem.Replenish (Event consumer: ReturnRequest → StockItem)

---

## Reviews & Ratings

### Review
- [ ] **Auto-moderation** — ML-based content check при ReviewSubmitted → auto-flag или auto-publish
- [ ] **Verified purchase check** — при ReviewSubmitted проверить Order context (Query: Review → Order)

### AggregateRating
- [ ] **Rating recalculation on review change** — Event consumer: Review → AggregateRating (при Publish/Delete/Reject)

---

## Notifications & Marketing

### Notification
- [ ] **Retry policy** — max 3 retry с exponential backoff при NotificationFailed
- [ ] **Deduplication** — по eventId + customerId (уже в спеке, нужна реализация)
- [ ] **Channel fallback** — если email failed → try SMS → try push

### Campaign
- [ ] **Scheduled launch** — cron-triggered Saga: при наступлении scheduledAt → CampaignStarted
- [ ] **Audience segmentation** — при CampaignStarted → query Customer context для выборки → batch Notification creation
- [ ] **Completion tracking** — когда все notifications sent → CampaignCompleted

### Promotion
- [ ] **Auto-expiration** — cron job: проверка expiresAt, переход Active → Expired
- [ ] **Price rollback** — при PromotionExpired → Product.ResetPrice (Event consumer)

### DiscountCode
- [ ] **Exhaustion detection** — при Redeem проверить usageCount >= maxUses → DiscountCodeExhausted (реализовано в Domain)
- [ ] **Auto-expiration** — cron job: проверка expiresAt → DiscountCodeExpired

---

## Cross-Context Sagas (Priority Order)

| Priority | Saga | Contexts | Trigger |
|----------|------|----------|---------|
| P0 | Order → Payment → Shipment | Order, Payment, Shipment | OrderConfirmed |
| P0 | Payment timeout | Order, Payment | PaymentInitiated + 30min TTL |
| P1 | Stock reservation | Cart, StockItem | ItemAddedToCart / OrderPlaced |
| P1 | Return → Refund → Restock | ReturnRequest, Payment, StockItem | ReturnCompleted |
| P2 | Invoice generation | Payment, Invoice, Notification | PaymentCompleted |
| P2 | Campaign execution | Campaign, Customer, Notification | CampaignStarted |
| P3 | GDPR export/deletion | Customer, Order, Review, Cart | CustomerRequest |
| P3 | Review → Rating sync | Review, AggregateRating | ReviewPublished/Deleted |
