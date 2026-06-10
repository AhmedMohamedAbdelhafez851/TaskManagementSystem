namespace TaskManagementSystem.BLL.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedToUserId { get; set; }
        public byte[] AttachmentContent { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentContentType { get; set; }
    }
}