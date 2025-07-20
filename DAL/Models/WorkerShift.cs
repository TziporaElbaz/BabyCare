namespace WEB_API.DAL.Models;

public partial class WorkerShift
{
    public int Id { get; set; }

    public int WorkerId { get; set; }

    public int ShiftId { get; set; }

    public virtual Shift Shift { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
    public WorkerShift(Shift shift, Worker worker)
    {
        WorkerId = worker.Id;
        ShiftId = shift.Id;
        Shift = shift;
        Worker = worker;
    }
    public WorkerShift() { }
}
