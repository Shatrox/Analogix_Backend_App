# Analogix Backend App

Backend API for a gaming-event platform where players can create profiles, publish events, subscribe, and build trust through ratings.

## Current Status (as of now)

Implemented:
- User registration
- User login with JWT
- `User` persistence with EF Core + SQL Server

Not implemented yet:
- Player profile
- Event lifecycle (create/update/delete)
- Event subscriptions and moderation
- Ratings (event/player)
- Account deletion flows and moderation safety

---

## Product Scope

### User Features
- Create account
- Create/update player profile:
  - Games you like
  - Games you want to learn
  - Player name
  - Gamer description
- Delete account
- Create event
- Update event
- Delete event
- Subscribe to event
- Accept/refuse subscriptions to your own event
- Rate event and/or player (Trust & Safety)

---

## Recommended Next Steps (Implementation Plan)

## Phase 1 — Foundation and Domain Modeling
1. **Define domain entities**
   - `PlayerProfile`
   - `Game`
   - `Event`
   - `EventSubscription`
   - `Rating`
2. **Define enums/value objects**
   - `SubscriptionStatus` (`Pending`, `Accepted`, `Refused`, `Cancelled`)
   - `EventVisibility` (`Public`, `Private`)
   - `RatingTargetType` (`Event`, `Player`)
3. **Create EF Core configurations + migrations**
   - Add indexes and unique constraints
   - Add FK relationships with delete behavior rules

> Goal: stable data model before API expansion.

## Phase 2 — Player Profile and Account Management
1. Add endpoints:
   - `GET /api/profile/me`
   - `PUT /api/profile/me`
   - `DELETE /api/account/me`
2. Validate and sanitize profile fields:
   - max lengths
   - restricted characters
   - profanity filter hook (optional now, required later)

> Goal: users can fully manage identity and profile data.

## Phase 3 — Event Management
1. Add event endpoints:
   - `POST /api/events`
   - `PUT /api/events/{id}`
   - `DELETE /api/events/{id}`
   - `GET /api/events` (filters: game, date, city, slots)
   - `GET /api/events/{id}`
2. Authorization rules:
   - only creator can edit/delete own event
3. Add event capacity and date validation:
   - future date required
   - max players > 1

> Goal: complete event CRUD with ownership checks.

## Phase 4 — Subscriptions Workflow
1. Add subscription endpoints:
   - `POST /api/events/{id}/subscriptions`
   - `GET /api/events/{id}/subscriptions` (owner only)
   - `PATCH /api/subscriptions/{id}/accept`
   - `PATCH /api/subscriptions/{id}/refuse`
   - `DELETE /api/subscriptions/{id}` (unsubscribe/cancel)
2. Rules:
   - prevent duplicate active subscriptions
   - prevent subscription to own event
   - auto-close when event capacity reached

> Goal: moderated join flow per event owner.

## Phase 5 — Trust & Safety (Ratings)
1. Add rating endpoints:
   - `POST /api/ratings`
   - `GET /api/players/{id}/ratings-summary`
   - `GET /api/events/{id}/ratings-summary`
2. Rules:
   - only participants can rate event/player
   - one rating per rater/target/context
   - immutable after cutoff or allow one edit window

> Goal: transparent trust scoring with abuse prevention.

## Phase 6 — Hardening
1. Add global exception handling middleware
2. Add `FluentValidation` (or equivalent)
3. Add pagination and sorting for list endpoints
4. Add audit fields:
   - `CreatedAtUtc`, `UpdatedAtUtc`, `CreatedBy`, `UpdatedBy`
5. Add observability:
   - structured logging
   - request correlation id
   - health checks (`/health`)

---

## Suggested Database Model (High Level)

- `Users` (already present)
- `PlayerProfiles` (1:1 with `Users`)
- `Games`
- `PlayerProfileGamesLiked` (M:N)
- `PlayerProfileGamesToLearn` (M:N)
- `Events` (N:1 creator user)
- `EventSubscriptions` (N:1 event, N:1 user)
- `Ratings` (N:1 rater user, polymorphic target by type/id)

---

## API and Security Standards

- JWT auth for protected routes
- Use `[Authorize]` on profile/event/subscription/rating endpoints
- Enforce ownership checks in service layer
- Never return password hashes in any DTO
- Add request/response DTOs per endpoint (do not expose entities directly)

---

## Testing Strategy (minimum)

1. **Unit tests**
   - Service business rules (ownership, duplicate subscription, rating constraints)
2. **Integration tests**
   - API + DB for critical flows:
     - register/login
     - create event
     - subscribe + accept/refuse
     - rate after participation
3. **Negative/security tests**
   - unauthorized update/delete
   - invalid token
   - duplicate actions

---

## Delivery Roadmap (Practical)

### Sprint 1
- Domain entities + migrations
- Profile endpoints
- Account deletion

### Sprint 2
- Event CRUD + listing filters
- Authorization policies

### Sprint 3
- Subscription workflow + owner moderation
- Capacity enforcement

### Sprint 4
- Ratings + trust summary endpoints
- Validation, tests, and API documentation polish

---

## Immediate Action Checklist (Start Here)

- [ ] Create entities and relationships for `PlayerProfile`, `Event`, `EventSubscription`, `Rating`
- [ ] Generate migration and update database
- [ ] Implement profile endpoints (`GET/PUT /me`)
- [ ] Implement event create/update/delete with ownership authorization
- [ ] Implement subscription request + accept/refuse flow
- [ ] Implement rating rules tied to participation
- [ ] Add unit/integration tests for each completed feature
- [ ] Update OpenAPI docs and example payloads