# Customer Journeys — EShop

## Overview

10 client journeys covering the full lifecycle of an e-commerce platform.
Each journey is verified by integration tests in `src/EShop.IntegrationTests/`.

---

## Journey 1: Catalog Setup

**Contexts:** Category → Product → StockItem

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Create root category "Electronics" | Category | CategoryCreated |
| 2 | Create subcategory "Laptops" under "Electronics" | Category | CategoryCreated |
| 3 | Create product "Gaming Laptop" in "Laptops" | Product | ProductCreated (Draft) |
| 4 | Add variant (SKU, price, attributes) | Product | VariantAdded |
| 5 | Add media (images) | Product | MediaAdded |
| 6 | Publish product | Product | ProductPublished |
| 7 | Initialize stock item for variant + warehouse | StockItem | StockInitialized |
| 8 | Replenish stock (set initial quantity) | StockItem | StockReplenished |

**Test:** `CatalogSetupJourneyTests`

---

## Journey 2: Customer Onboarding

**Contexts:** Customer → Session

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Register customer (email, name, password hash) | Customer | CustomerRegistered (Unverified) |
| 2 | Verify email | Customer | EmailVerified (Active) |
| 3 | Update profile (phone, address) | Customer | ProfileUpdated |
| 4 | Login (create session) | Session | CustomerLoggedIn (Active session) |
| 5 | Create second session (another device) | Session | CustomerLoggedIn |
| 6 | Verify max 5 active sessions enforced | Session | Invariant check |
| 7 | Revoke specific session | Session | SessionRevoked |

**Test:** `CustomerOnboardingJourneyTests`

---

## Journey 3: Browse & Cart

**Contexts:** Product → Cart → DiscountCode

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Browse products (read model) | Product | Product data retrieved |
| 2 | Add item to cart | Cart | ItemAddedToCart (Active) |
| 3 | Add second item | Cart | ItemAddedToCart |
| 4 | Update item quantity | Cart | CartUpdated |
| 5 | Remove item | Cart | CartUpdated |
| 6 | Generate discount code | DiscountCode | DiscountCodeGenerated (Active) |
| 7 | Validate discount code | DiscountCode | DiscountCodeValidated |
| 8 | Apply promo code to cart | Cart | PromoCodeApplied |
| 9 | Verify max 50 items, qty 1-99 enforced | Cart | Invariant checks |

**Test:** `BrowseAndCartJourneyTests`

---

## Journey 4: Purchase (Happy Path)

**Contexts:** Cart → Order → StockItem → Payment → Invoice → Shipment

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Place order from cart | Order | OrderConfirmed (Placed) |
| 2 | Cart transitions to CheckedOut | Cart | OrderPlaced |
| 3 | Reserve stock for each line item | StockItem | StockReserved |
| 4 | Initiate payment | Payment | PaymentInitiated |
| 5 | Complete payment | Payment | PaymentCompleted |
| 6 | Order transitions to Paid | Order | OrderPaid |
| 7 | Generate invoice | Invoice | InvoiceGenerated |
| 8 | Send invoice | Invoice | InvoiceSent |
| 9 | Create shipment | Shipment | ShipmentCreated (Pending) |
| 10 | Pack shipment | Shipment | ShipmentPacked |
| 11 | Dispatch shipment | Shipment | ShipmentDispatched |
| 12 | Mark in transit | Shipment | ShipmentInTransit |
| 13 | Deliver shipment | Shipment | ShipmentDelivered |
| 14 | Order transitions to Delivered | Order | OrderDelivered |
| 15 | Redeem discount code (if used) | DiscountCode | DiscountCodeRedeemed |

**Test:** `PurchaseHappyPathJourneyTests`

---

## Journey 5: Order Cancellation (Before Payment)

**Contexts:** Order → StockItem → Payment

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Place order | Order | OrderConfirmed (Placed) |
| 2 | Reserve stock | StockItem | StockReserved |
| 3 | Cancel order (status = Placed) | Order | OrderCancelled |
| 4 | Release reserved stock | StockItem | StockReleased |
| 5 | Void payment (if initiated) | Payment | PaymentVoided |

**Test:** `OrderCancellationJourneyTests.CancelPlacedOrder_*`

---

## Journey 6: Paid Order Cancellation (After Payment)

**Contexts:** Order → StockItem → Payment

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Place and pay order | Order | OrderPaid |
| 2 | Cancel paid order | Order | OrderCancelled |
| 3 | Release reserved stock | StockItem | StockReleased |
| 4 | Request refund | Payment | RefundIssued |

**Test:** `OrderCancellationJourneyTests.CancelPaidOrder_*`

---

## Journey 7: Return Flow

**Contexts:** Shipment → ReturnRequest → StockItem → Payment

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Customer requests return (EU 14-day rule) | ReturnRequest | ReturnRequested |
| 2 | Approve return request | ReturnRequest | ReturnApproved |
| 3 | Customer ships return | ReturnRequest | ReturnShipped |
| 4 | Warehouse receives return | ReturnRequest | ReturnReceived |
| 5 | Complete return (inspection passed) | ReturnRequest | ReturnCompleted |
| 6 | Replenish stock | StockItem | StockReplenished |
| 7 | Issue refund | Payment | RefundIssued |

**Alternative:** Return rejected after inspection → `ReturnItemRejected`

**Test:** `ReturnFlowJourneyTests`

---

## Journey 8: Review Flow

**Contexts:** Shipment → Review → AggregateRating

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Verify shipment delivered | Shipment | Precondition |
| 2 | Submit review (rating 1-5, text 10-5000 chars) | Review | ReviewSubmitted |
| 3 | Approve review (moderation) | Review | ReviewPublished |
| 4 | Initialize aggregate rating | AggregateRating | RatingInitialized (Pending) |
| 5 | Recalculate rating (add review data) | AggregateRating | AggregateRatingUpdated |
| 6 | Verify weighted average (verified purchase ×2) | AggregateRating | Rating calculated |
| 7 | Activate rating (min 3 reviews threshold) | AggregateRating | Status → Active |

**Test:** `ReviewFlowJourneyTests`

---

## Journey 9: Marketing Campaign

**Contexts:** Promotion → DiscountCode → Campaign → Notification

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Create promotion (discount %, conditions, period) | Promotion | PromotionCreated (Draft) |
| 2 | Activate promotion | Promotion | PromotionActivated (Active) |
| 3 | Generate discount codes for promotion | DiscountCode | DiscountCodeGenerated |
| 4 | Create campaign (target segment, schedule) | Campaign | CampaignCreated (Draft) |
| 5 | Schedule campaign | Campaign | CampaignScheduled |
| 6 | Start sending | Campaign | CampaignStarted (Sending) |
| 7 | Create marketing notification | Notification | NotificationCreated |
| 8 | Render notification (template + data) | Notification | NotificationRendered |
| 9 | Send notification (requires consent) | Notification | NotificationSent |
| 10 | Complete campaign | Campaign | CampaignCompleted |

**Test:** `MarketingCampaignJourneyTests`

---

## Journey 10: Notification on Events

**Contexts:** Order/Payment/Shipment → Notification

| Step | Action | Context | Result |
|------|--------|---------|--------|
| 1 | Domain event triggers notification | — | Event received |
| 2 | Create transactional notification | Notification | NotificationCreated |
| 3 | Render notification content | Notification | NotificationRendered |
| 4 | Send notification (transactional — no consent needed) | Notification | NotificationSent |

**Retry flow:**
| Step | Action | Result |
|------|--------|--------|
| 5 | Send fails | NotificationFailed |
| 6 | Retry (max 3 attempts) | RetryCount incremented |
| 7 | Re-render and re-send | NotificationSent |

**Test:** `NotificationJourneyTests`

---

## Bounded Context Map

```
┌─────────────────────────────────────────────────────────────────┐
│                    Catalog & Inventory (Core)                   │
│  ┌──────────┐    ┌──────────┐    ┌──────────┐                  │
│  │ Category │───→│ Product  │───→│StockItem │                  │
│  └──────────┘    └──────────┘    └──────────┘                  │
└─────────────────────────────────────────────────────────────────┘
                          │
┌─────────────────┐       │       ┌───────────────────────────────┐
│ Identity (Gen.) │       │       │      Shopping (Core)          │
│ ┌────────┐      │       ▼       │  ┌──────┐    ┌───────┐       │
│ │Customer│      │    Browse     │  │ Cart │───→│ Order │       │
│ └────────┘      │               │  └──────┘    └───────┘       │
│ ┌────────┐      │               └───────────────────────────────┘
│ │Session │      │                       │
│ └────────┘      │                       ▼
└─────────────────┘       ┌───────────────────────────────────────┐
                          │    Payment & Billing (Core)           │
                          │  ┌─────────┐    ┌─────────┐          │
                          │  │ Payment │    │ Invoice │          │
                          │  └─────────┘    └─────────┘          │
                          └───────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Fulfillment (Supporting)                     │
│  ┌──────────┐    ┌──────────────┐                              │
│  │ Shipment │    │ReturnRequest │                              │
│  └──────────┘    └──────────────┘                              │
└─────────────────────────────────────────────────────────────────┘
         │
         ▼
┌─────────────────────────────────────────────────────────────────┐
│                Reviews & Ratings (Supporting)                   │
│  ┌────────┐    ┌─────────────────┐                             │
│  │ Review │───→│AggregateRating  │                             │
│  └────────┘    └─────────────────┘                             │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│              Notifications & Marketing (Supporting)             │
│  ┌────────────┐  ┌──────────┐  ┌──────────┐  ┌──────────────┐ │
│  │Notification│  │ Campaign │  │Promotion │  │DiscountCode  │ │
│  └────────────┘  └──────────┘  └──────────┘  └──────────────┘ │
└─────────────────────────────────────────────────────────────────┘
```

## State Machines Summary

| Context | States |
|---------|--------|
| Product | Draft → Published → Archived → Draft |
| Category | Active ↔ Inactive |
| StockItem | InStock → LowStock → OutOfStock |
| Customer | Unverified → Active → Deactivated |
| Session | Active → Expired / Revoked |
| Cart | Active → CheckedOut / Abandoned |
| Order | Placed → Paid → Shipped → Delivered / Cancelled |
| Payment | Initiated → Completed → PartiallyRefunded / FullyRefunded, Failed, Voided |
| Invoice | Generated → Sent |
| Shipment | Pending → Packed → Shipped → InTransit → Delivered |
| ReturnRequest | Requested → Approved → ReturnShipped → Received → Completed / RejectedAfterInspection |
| Review | Submitted → Published → Flagged → Rejected / Deleted |
| AggregateRating | Pending ↔ Active |
| Notification | Created → Rendered → Sent / Failed |
| Campaign | Draft → Scheduled → Sending → Completed / Cancelled |
| Promotion | Draft → Active → Expired |
| DiscountCode | Active → Exhausted / Expired / Deactivated |
