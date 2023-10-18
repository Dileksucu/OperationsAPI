namespace OperationsAPI.Models
{
    // Genelde varlıklar Remove() metodu ile tamamen kalıcı olarak silinmez
    // çünkü loglama gibi kanuni durumlar ve veri kaybı olmaması için her zaman verinin tutulması önemlidir
    // onun yerine veriyi silindi olarak işaretleyerek sanki remove edilmiş gibi davranış göstermesini sağlarız
    public interface ISoftDeleteEntity
    {
        bool IsDeleted { get; set; }
    }
}
