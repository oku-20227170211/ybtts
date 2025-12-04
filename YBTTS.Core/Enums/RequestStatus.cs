namespace YBTTS.Core.Enums;

public enum RequestStatus
{
    Pending = 0,        // Beklemede
    Approved = 1,       // Onaylandı
    Assigned = 2,       // Bakım personeline atandı
    InProgress = 3,     // İşlem sürüyor
    Completed = 4,      // Tamamlandı
    Rejected = 5        // Reddedildi
}