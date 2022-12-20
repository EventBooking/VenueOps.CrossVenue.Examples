# VenueOps.CrossVenue.Examples

## CVC Example

See the `output` folder for payload examples. Each payload contains `ClusterCode` and `TenantId`. This is enough information to uniquely identify a tenant.

For the most part, creation/modification dates (or who did it) is not available directly on domain objects in VenueOps. That is typically handled via activity logs.

### VenueChangePayload.json

This payload will be sent any time a venue is added or modified.

Not currently exposed by the Open API:

- sort_order

### RoomChangePayload.json

This payload will be sent any time a room is added or modified.

Not currently exposed by the Open API:

- room_sort_order
- group_name
- group_sort_order

### EventChangePayload.json

This payload will be sent any time an event or its booked spaces is added or modified.

Not currently exposed by the Open API:

- master_event_type
- is_live_entertainment
