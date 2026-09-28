namespace RentACar.Core.Entities.Concrete
{
    // SENİOR NOTU: Sınıfın başına 'abstract' yazdık. Neden?
    // Çünkü 'BaseEntity' tek başına anlamsızdır (new BaseEntity() yazılamaz). 
    // O sadece Brand, Color gibi gerçek tablolara gen aktaran bir DNA'dır.
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Bir veri ilk eklendiğinde güncellenmiş veya silinmiş olamaz, o yüzden boş (null) kalabilmelidir.
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

        // CONSTRUCTOR (Yapıcı Metot - İlk İşçi)
        // ctor yazıp iki kere TAB tuşuna basarsan bu bloğu otomatik açar.
        public BaseEntity()
        {
            // Nesne RAM'de doğduğu milisaniye, doğum belgesini (tarihi) mühürle!
            CreatedDate = DateTime.UtcNow;

            // Yeni doğan bir şey çöpte olamaz, varsayılan olarak false yapıyoruz.
            IsDeleted = false;
        }
    }
}