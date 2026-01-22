# PhotographerPlatform.Store - Implementation Roadmap

## Overview
This roadmap outlines the implementation plan for the Store microservice, which handles product catalog management, shopping cart, checkout, discounts, and order management.

---

## Phase 1: Foundation & Infrastructure

### 1.1 Project Setup
- [ ] Initialize .NET project structure with clean architecture
- [ ] Configure dependency injection container
- [ ] Set up logging and telemetry infrastructure
- [ ] Configure database context and migrations
- [ ] Set up message broker for order events

### 1.2 Data Models & Storage
- [ ] Create `Product` entity (name, type, price, description, images)
- [ ] Create `ProductVariant` entity (product, size, options, price)
- [ ] Create `Cart` entity (user/session, items, created, updated)
- [ ] Create `CartItem` entity (cart, product, quantity, options)
- [ ] Create `Order` entity (user, items, total, status, dates)
- [ ] Create `OrderItem` entity (order, product, quantity, price)
- [ ] Create `Discount` entity (code, type, value, rules, dates)
- [ ] Create `Tax` entity (region, rate, rules)
- [ ] Set up database indexes and relationships

### 1.3 Pricing Infrastructure
- [ ] Implement price calculation engine
- [ ] Create tax calculation service
- [ ] Build currency handling
- [ ] Set up pricing rules framework

---

## Phase 2: Product Catalog (Requirement 4.1)

### 2.1 Product Types
- [ ] Implement print product type
- [ ] Implement digital download product type
- [ ] Create product type abstraction
- [ ] Add product type validation rules

### 2.2 Product Management
- [ ] Create product creation endpoint
- [ ] Implement product update
- [ ] Add product deletion/archiving
- [ ] Create product duplication

### 2.3 Product Pricing
- [ ] Implement base price setting
- [ ] Create variant pricing
- [ ] Add bulk pricing rules
- [ ] Implement currency support

### 2.4 Tax Configuration
- [ ] Create tax rate management
- [ ] Implement regional tax rules
- [ ] Add tax exemption handling
- [ ] Create tax reporting

### 2.5 Product Images & Media
- [ ] Create product image upload
- [ ] Implement image ordering
- [ ] Add product description formatting
- [ ] Create product preview generation

### 2.6 Product Catalog API
- [ ] Create `POST /api/products` - Create product
- [ ] Create `GET /api/products` - List products
- [ ] Create `GET /api/products/{id}` - Product details
- [ ] Create `PUT /api/products/{id}` - Update product
- [ ] Create `DELETE /api/products/{id}` - Delete product
- [ ] Create `POST /api/products/{id}/variants` - Add variant
- [ ] Create `GET /api/products/catalog` - Public catalog

### 2.7 Product Catalog Tests
- [ ] Unit tests for product creation
- [ ] Unit tests for pricing calculation
- [ ] Integration tests for catalog
- [ ] Acceptance test: "Add a digital product" scenario

---

## Phase 3: Cart & Checkout (Requirement 4.2)

### 3.1 Shopping Cart
- [ ] Create cart initialization
- [ ] Implement add to cart
- [ ] Create update cart item quantity
- [ ] Implement remove from cart
- [ ] Build cart persistence (session/user)
- [ ] Add cart expiration handling

### 3.2 Cart Calculations
- [ ] Calculate subtotal
- [ ] Apply discounts
- [ ] Calculate taxes
- [ ] Calculate shipping (if applicable)
- [ ] Generate order total

### 3.3 Checkout Flow
- [ ] Create checkout initiation
- [ ] Collect billing information
- [ ] Collect shipping information (prints)
- [ ] Validate cart before checkout
- [ ] Create payment intent

### 3.4 Payment Processing
- [ ] Integrate with Integrations service for payment
- [ ] Handle payment success
- [ ] Handle payment failure
- [ ] Create payment retry flow

### 3.5 Order Creation
- [ ] Create order from cart
- [ ] Generate order number
- [ ] Set initial order status
- [ ] Clear cart after order
- [ ] Trigger order confirmation email

### 3.6 Cart & Checkout API
- [ ] Create `GET /api/cart` - Get current cart
- [ ] Create `POST /api/cart/items` - Add item
- [ ] Create `PUT /api/cart/items/{id}` - Update quantity
- [ ] Create `DELETE /api/cart/items/{id}` - Remove item
- [ ] Create `DELETE /api/cart` - Clear cart
- [ ] Create `POST /api/checkout` - Initiate checkout
- [ ] Create `POST /api/checkout/complete` - Complete order
- [ ] Create `GET /api/checkout/summary` - Order summary

### 3.7 Cart & Checkout Tests
- [ ] Unit tests for cart operations
- [ ] Unit tests for calculations
- [ ] Integration tests for checkout flow
- [ ] Acceptance test: "Client completes a purchase" scenario

---

## Phase 4: Coupons & Discounts (Requirement 4.3)

### 4.1 Discount Types
- [ ] Implement percentage discount
- [ ] Implement fixed amount discount
- [ ] Create free shipping discount
- [ ] Add buy-one-get-one (BOGO) discount

### 4.2 Discount Rules
- [ ] Create minimum order amount rule
- [ ] Implement product/category restrictions
- [ ] Add usage limit per code
- [ ] Create usage limit per customer
- [ ] Implement date range restrictions

### 4.3 Discount Management
- [ ] Create discount code generation
- [ ] Implement discount editing
- [ ] Add discount deactivation
- [ ] Create discount analytics

### 4.4 Discount Application
- [ ] Create discount validation service
- [ ] Implement discount application to cart
- [ ] Handle multiple discount stacking rules
- [ ] Create discount error messages

### 4.5 Discount API
- [ ] Create `POST /api/discounts` - Create discount
- [ ] Create `GET /api/discounts` - List discounts
- [ ] Create `GET /api/discounts/{id}` - Discount details
- [ ] Create `PUT /api/discounts/{id}` - Update discount
- [ ] Create `DELETE /api/discounts/{id}` - Delete discount
- [ ] Create `POST /api/cart/discount` - Apply to cart
- [ ] Create `DELETE /api/cart/discount` - Remove from cart
- [ ] Create `POST /api/discounts/validate` - Validate code

### 4.6 Discount Tests
- [ ] Unit tests for discount rules
- [ ] Unit tests for discount application
- [ ] Integration tests for discount flow
- [ ] Acceptance test: "Apply a valid discount" scenario

---

## Phase 5: Order Management (Requirement 4.4)

### 5.1 Order Status Workflow
- [ ] Define order status states (pending, paid, processing, shipped, delivered, cancelled, refunded)
- [ ] Implement status transitions
- [ ] Create status change validation
- [ ] Add status change notifications

### 5.2 Order Fulfillment
- [ ] Create fulfillment queue
- [ ] Implement mark as processing
- [ ] Add shipping information capture
- [ ] Create mark as shipped
- [ ] Implement mark as delivered

### 5.3 Print Order Fulfillment
- [ ] Integrate with lab fulfillment (Integrations service)
- [ ] Track lab order status
- [ ] Handle fulfillment updates
- [ ] Create fulfillment error handling

### 5.4 Digital Order Fulfillment
- [ ] Generate download links
- [ ] Set download expiration
- [ ] Track download count
- [ ] Create re-download functionality

### 5.5 Refund Processing
- [ ] Create refund initiation
- [ ] Integrate with payment refund
- [ ] Handle partial refunds
- [ ] Update order status
- [ ] Create refund notifications

### 5.6 Order Management API
- [ ] Create `GET /api/orders` - List orders
- [ ] Create `GET /api/orders/{id}` - Order details
- [ ] Create `PUT /api/orders/{id}/status` - Update status
- [ ] Create `POST /api/orders/{id}/fulfill` - Mark fulfilled
- [ ] Create `POST /api/orders/{id}/ship` - Add shipping info
- [ ] Create `POST /api/orders/{id}/refund` - Process refund
- [ ] Create `GET /api/orders/{id}/downloads` - Download links

### 5.7 Order Management Tests
- [ ] Unit tests for status transitions
- [ ] Unit tests for fulfillment logic
- [ ] Integration tests for refund flow
- [ ] Acceptance test: "Mark an order as fulfilled" scenario

---

## Phase 6: Customer Order Experience

### 6.1 Order History
- [ ] Create customer order list
- [ ] Implement order detail view
- [ ] Add order tracking
- [ ] Create reorder functionality

### 6.2 Order Notifications
- [ ] Send order confirmation email
- [ ] Send shipping notification
- [ ] Send delivery confirmation
- [ ] Send download ready notification

### 6.3 Customer API
- [ ] Create `GET /api/me/orders` - My orders
- [ ] Create `GET /api/me/orders/{id}` - My order details
- [ ] Create `GET /api/me/orders/{id}/track` - Track order

---

## Phase 7: Advanced Store Features

### 7.1 Inventory Management (If Applicable)
- [ ] Track product stock levels
- [ ] Implement low stock alerts
- [ ] Create backorder handling
- [ ] Add inventory reservations

### 7.2 Shipping Configuration
- [ ] Create shipping zones
- [ ] Implement shipping rate calculation
- [ ] Add flat rate shipping
- [ ] Create free shipping thresholds

### 7.3 Store Analytics
- [ ] Track conversion rates
- [ ] Monitor cart abandonment
- [ ] Analyze product performance
- [ ] Create sales reports

---

## Phase 8: Store Configuration

### 8.1 Store Settings
- [ ] Configure currency
- [ ] Set tax display preferences
- [ ] Configure checkout options
- [ ] Add store policies

### 8.2 Settings API
- [ ] Create `GET /api/store/settings` - Get settings
- [ ] Create `PUT /api/store/settings` - Update settings

---

## Dependencies

| Dependency | Type | Description |
|------------|------|-------------|
| Identity Service | Internal | User authentication |
| Galleries Service | Internal | Image selection for products |
| Integrations Service | Internal | Payment & lab fulfillment |
| Communication Service | Internal | Order notifications |
| Analytics Service | Internal | Sales tracking |

---

## API Endpoints Summary

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/products` | GET/POST | List/create products |
| `/api/products/{id}` | GET/PUT/DELETE | Product CRUD |
| `/api/products/{id}/variants` | POST | Add variant |
| `/api/products/catalog` | GET | Public catalog |
| `/api/cart` | GET/DELETE | Get/clear cart |
| `/api/cart/items` | POST | Add to cart |
| `/api/cart/items/{id}` | PUT/DELETE | Update/remove item |
| `/api/cart/discount` | POST/DELETE | Apply/remove discount |
| `/api/checkout` | POST | Initiate checkout |
| `/api/checkout/complete` | POST | Complete purchase |
| `/api/discounts` | GET/POST | List/create discounts |
| `/api/discounts/{id}` | GET/PUT/DELETE | Discount CRUD |
| `/api/discounts/validate` | POST | Validate code |
| `/api/orders` | GET | List orders |
| `/api/orders/{id}` | GET | Order details |
| `/api/orders/{id}/status` | PUT | Update status |
| `/api/orders/{id}/fulfill` | POST | Mark fulfilled |
| `/api/orders/{id}/refund` | POST | Process refund |
| `/api/me/orders` | GET | Customer orders |

---

## Order Status Flow

```
[Created] → [Pending Payment] → [Paid] → [Processing] → [Shipped] → [Delivered]
                ↓                  ↓           ↓
           [Cancelled]      [Refunded]   [Partially Refunded]
```

---

## Success Criteria

- [ ] Digital and print products can be created with pricing
- [ ] Products appear in the product catalog
- [ ] Clients can add items to cart and complete checkout
- [ ] Order confirmations are sent after purchase
- [ ] Valid discount codes reduce order totals
- [ ] Invalid/expired codes show appropriate errors
- [ ] Orders can be marked as fulfilled with status updates
- [ ] All acceptance criteria from requirements.md are passing
