# VenueOps.CrossVenue.Examples

## CVC Reader

This tool demonstrates how to download the data models from S3. You can use the `download.ps1` script to set the necessary environment variables and then run the tool. Before running it, you wil need to set these this variable:

```
$env:CvcDir = 'YOUR LOCAL DIRECTORY WHERE THE FILES SHOULD BE PLACED'
$env:CvcAwsSecretKey = 'SECRET KEY SENT SEPARATELY'
```

Once you have set these and run the script, you should see a set of folders/files in the specified directory. There is also code demonstrating how to download and deserialize objects directly from S3 without having to write them out locally.

## Payload Examples

See the `output` folder for payload examples. Each payload contains `ClusterCode` and `TenantId`. This is enough information to uniquely identify a tenant.

For the most part, creation/modification dates (or who did it) is not available directly on domain objects in VenueOps. That is typically handled via activity logs.

### Incremental examples

See the `output/incremental` folder.

#### VenueChangePayload.json

This payload will be sent any time a venue is added or modified.

Not currently exposed by the Open API:

- sort_order

#### RoomChangePayload.json

This payload will be sent any time a room is added or modified.

Not currently exposed by the Open API:

- room_sort_order
- group_name
- group_sort_order

#### EventChangePayload.json

This payload will be sent any time an event or its booked spaces is added or modified.

Not currently exposed by the Open API:

- master_event_type
- is_live_entertainment

### Bulk examples

See the `output/bulk` folder. These files could end up being quite large, so would likely be sent via Azure BLOB Storage (or AWS S3).

#### SetupBulkPayload.json

This contains _all_ necessary setup information from the specific tenant. Right now it contains only venues and rooms.

#### EventBulkPayload-0.json, EventBulkPayload-1.json, etc.

These files would contain _all_ events in the specific tenant, paginated in order to limit the maximum size of each file.
