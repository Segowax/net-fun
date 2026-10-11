using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Listener.Models
{
    [Table("sensor_data")]
    class SensorData : BaseModel
    {
        public SensorData() { }

        [Column("sensor_id")]
        public string SensorId { get; set; } = string.Empty;
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Column("enqueued_time")]
        public DateTime EnqueuedTime { get; set; }
        [Column("value")]
        public virtual object? Value { get; set; }
    }
}
