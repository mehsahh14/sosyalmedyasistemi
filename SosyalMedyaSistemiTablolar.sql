
create database SosyalMedyaSistemi;
use SosyalMedyaSistemi;


CREATE TABLE  kullanicilar(
	kullanici_id INT AUTO_INCREMENT PRIMARY KEY NOT NULL ,
	kullanicitakmaadi VARCHAR(30) UNIQUE NOT NULL,
    ad VARCHAR(70) NOT NULL,
    soyad VARCHAR(70) NOT NULL,
	email VARCHAR(255) UNIQUE NOT NULL CHECK(email like('%@%bartin.edu.tr%')), 
	sifre VARCHAR(255) NOT NULL,
	olusturulma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE gonderiler (
	gonderi_id INT AUTO_INCREMENT PRIMARY KEY NOT NULL ,
	fotograf VARCHAR(255) NOT NULL,
    aciklama TEXT,
	yukleme_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    kullanici_id INT NOT NULL,
	
    FOREIGN KEY (kullanici_id) REFERENCES kullanicilar(kullanici_id) ON DELETE CASCADE ON UPDATE CASCADE

);
ALTER TABLE gonderiler ADD begeni_sayisi INT DEFAULT 0;
ALTER TABLE gonderiler MODIFY COLUMN fotograf VARCHAR(255) NULL;
 
CREATE TABLE begeniler (
    begeni_id INT AUTO_INCREMENT PRIMARY KEY,
    begeni_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    kullanici_id INT NOT NULL,
    gonderi_id INT NOT NULL,
   
    FOREIGN KEY (kullanici_id) REFERENCES kullanicilar(kullanici_id) ON DELETE CASCADE,
    FOREIGN KEY (gonderi_id) REFERENCES gonderiler(gonderi_id) ON DELETE CASCADE,
    UNIQUE (kullanici_id, gonderi_id) 
    
);

CREATE TABLE takipler (
    takip_id INT AUTO_INCREMENT PRIMARY KEY,
    takip_eden_id INT NOT NULL,
    takip_edilen_id INT NOT NULL,
    takip_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (takip_eden_id) REFERENCES kullanicilar(kullanici_id) ON DELETE CASCADE,
    FOREIGN KEY (takip_edilen_id) REFERENCES kullanicilar(kullanici_id) ON DELETE CASCADE,
    UNIQUE (takip_eden_id, takip_edilen_id) 

);

CREATE TABLE yorumlar (
    yorum_id INT AUTO_INCREMENT PRIMARY KEY,
    yorum TEXT NOT NULL,
    yorum_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    kullanici_id INT NOT NULL,
    gonderi_id INT NOT NULL,

    FOREIGN KEY (kullanici_id) REFERENCES kullanicilar(kullanici_id) ON DELETE CASCADE,
    FOREIGN KEY (gonderi_id) REFERENCES gonderiler(gonderi_id) ON DELETE CASCADE
);

CREATE TABLE bildirimler (
    bildirim_id INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
    tetikleyen_id INT NOT NULL, 
    alici_id INT NOT NULL,     
    gonderi_id INT NULL,       
    bildirim_turu VARCHAR(20) NOT NULL, 
    icerik TEXT NULL,          
    is_read BOOLEAN DEFAULT FALSE,
    olusturulma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (tetikleyen_id) REFERENCES kullanicilar(kullanici_id) ON DELETE CASCADE,
    FOREIGN KEY (alici_id) REFERENCES kullanicilar(kullanici_id) ON DELETE CASCADE,
    FOREIGN KEY (gonderi_id) REFERENCES gonderiler(gonderi_id) ON DELETE CASCADE
);
CREATE TABLE kulupler (
    kulup_id INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
    kulup_adi VARCHAR(100) UNIQUE NOT NULL,
    aciklama TEXT,
    logo VARCHAR(255) NULL,
    olusturan_id INT NOT NULL, -- Kulübü açan başkan/hoca id'si
    olusturulma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (olusturan_id) REFERENCES kullanicilar(kullanici_id) ON DELETE CASCADE
);


CREATE TABLE kulup_uyeleri (
    uye_id INT AUTO_INCREMENT PRIMARY KEY,
    kulup_id INT NOT NULL,
    kullanici_id INT NOT NULL,
    katilim_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (kulup_id, kullanici_id), 
    FOREIGN KEY (kulup_id) REFERENCES kulupler(kulup_id) ON DELETE CASCADE,
    FOREIGN KEY (kullanici_id) REFERENCES kullanicilar(kullanici_id) ON DELETE CASCADE
);


CREATE TABLE etkinlikler (
    etkinlik_id INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
    kulup_id INT NOT NULL,
    etkinlik_adi VARCHAR(155) NOT NULL,
    detay TEXT NOT NULL,
    etkinlik_tarihi DATETIME NOT NULL,
    yer VARCHAR(155) NOT NULL,
    olusturulma_tarihi TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (kulup_id) REFERENCES kulupler(kulup_id) ON DELETE CASCADE
);

select * from kulup_uyeleri;
select *from kullanicilar;
select *from gonderiler;
select *from yorumlar;