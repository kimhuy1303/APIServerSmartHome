namespace APIServerSmartHome.DTOs
{
    public class RoomDTO
    {
        public string? RoomName { get; set; }
    }

    public class RoomResponseDTO
    {
        public int Id { get; set; }
        public string? RoomName { get; set; }
        public int? AmountOfDevice { get; set; }
    }
}
