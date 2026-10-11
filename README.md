# Listener

A .NET 10 console application that reads messages from a serial port (RS-232), buffers valid JSON data, and saves it to the `sensor_data` table in Supabase.

## Requirements

- .NET 10 SDK
- An available serial port and a device that sends data
- A Supabase project with a `sensor_data` table

## Configuration

Before running the application, configure `Listener/appsettings.json`:

```json
{
  "Supabase": {
	"ProjectURL": "https://<project-id>.supabase.co",
	"ApiKey": "<supabase-api-key>"
  },
  "Rs232": {
	"Port": "<your-port>",
	"BaudRate": <baud rate you want>
  }
}
```

Set `Port` and `BaudRate` to match your device configuration. The application loads `appsettings.json` from the working directory; the file is also copied to the output directory during the build.

Do not commit a real Supabase key to the repository. If a key has already been exposed or committed, revoke it in Supabase and generate a new one.

## Message format

The device should send each message as a JSON object terminated by the `@` character. Example:

```text
{"sensor_id":"sensor-01","name":"temperature","value":21.5}@
```

The object must contain the `sensor_id`, `name`, and `value` fields. Before inserting the record, the application sets `enqueued_time` to the current UTC time. Data that is not text beginning with `{` and ending with `}` is not added to the buffer.

The Supabase `sensor_data` table should contain columns corresponding to the application model:

| Column | Description |
| --- | --- |
| `sensor_id` | Sensor identifier |
| `name` | Measurement name |
| `enqueued_time` | Enqueue time (UTC) |
| `value` | Measurement value |

Also ensure that your access policies (RLS) and API key permissions allow the application to insert records.

## Run the application

From the repository root:

```powershell
dotnet run --project .\Listener\Listener.csproj
```

To build the project:

```powershell
dotnet build .\Listener\Listener.csproj
```

## Controls

After the application starts:

- `L` — displays RS-232 read or data validation errors,
- `S` — displays Supabase initialization or write errors,
- `X` — exits the application.
