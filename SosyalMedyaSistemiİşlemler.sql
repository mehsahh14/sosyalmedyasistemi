use SosyalMedyaSistemi;

CALL get_insertKullanici('Mehmet_14', 'Mehmet', 'Şahin', '24010310072@ogrenci.bartin.edu.tr', '123456789');
CALL get_girisyap('24010310072@ogrenci.bartin.edu.tr');
CALL get_kullanici_detay(1);

CALL get_gonderiekle(1, '/uploads/1.png', 'Harika bir gün!');
CALL get_gonderiler();
CALL get_gonderisil(20, 1);

CALL get_takipet(1, 2);
CALL get_takibibirak(1, 2);
CALL get_takip_edilenler(1);
CALL get_takipciler(1);

CALL get_yorumyap(1, 19, 'Harika bir paylaşım!');
CALL get_yorumlar(19);

CALL get_begeniekle(1, 19); 
CALL get_begenikaldir(1, 19);

CALL get_bildirimekle(2, 1, 19, 'begeni', 'Gönderini beğendi');
CALL get_bildirimler(1); 
CALL get_bildirimokundu(5, 1);

CALL get_kulupler();
CALL get_kulup_detay(1); 
CALL get_kulup_katil(1, 2); 
CALL get_kulupolustur('Roket Kulübü', 'Yüksek irtifa roket denemeleri', 1);
CALL get_kulup_uyeleri(1); 
CALL get_kulup_etkinlikleri(1); 

CALL get_etkinlikler();
CALL get_etkinlikekle(1, 'Teknofest Toplantısı', 'A3 kategorisi için tasarım analizi', '2026-06-01 10:00:00', 'Mühendislik Binası');

CALL kullanici_ara('mehmet');
CALL update_kullanici_profil(1, 'Mehmetsahin14', 'Mehmet ', 'Şahin');
CALL kullanici_sil(6);




-- TRIGGER KISIMLARIMI BURAYA YAZDIM 

DELIMITER //

CREATE TRIGGER trg_begeni_arttir
AFTER INSERT ON begeniler
FOR EACH ROW
BEGIN
    UPDATE gonderiler
    SET begeni_sayisi = begeni_sayisi + 1
    WHERE gonderi_id = NEW.gonderi_id;
END$$

DELIMITER ;



DELIMITER //

CREATE TRIGGER trg_begeni_azalt
AFTER DELETE ON begeniler
FOR EACH ROW
BEGIN
    UPDATE gonderiler
    SET begeni_sayisi = begeni_sayisi - 1
    WHERE gonderi_id = OLD.gonderi_id;
END$$

DELIMITER ;


DELIMITER //
CREATE TRIGGER TRG_BegeniBildirim
AFTER INSERT ON begeniler
FOR EACH ROW
BEGIN
    DECLARE v_gonderi_sahibi INT;
    
   
    SELECT kullanici_id INTO v_gonderi_sahibi 
    FROM gonderiler 
    WHERE gonderi_id = NEW.gonderi_id;
    
    
    IF NEW.kullanici_id <> v_gonderi_sahibi THEN
        CALL get_bildirimekle(NEW.kullanici_id, v_gonderi_sahibi, NEW.gonderi_id, 'begeni', NULL);
    END IF;
END //
DELIMITER ;




DELIMITER //
CREATE TRIGGER TRG_YorumBildirim
AFTER INSERT ON yorumlar
FOR EACH ROW
BEGIN
    DECLARE v_gonderi_sahibi INT;
    
   
    SELECT kullanici_id INTO v_gonderi_sahibi 
    FROM gonderiler 
    WHERE gonderi_id = NEW.gonderi_id;
    
    
    IF NEW.kullanici_id <> v_gonderi_sahibi THEN
        CALL get_bildirimekle(NEW.kullanici_id, v_gonderi_sahibi, NEW.gonderi_id, 'yorum', NEW.yorum);
    END IF;
END //
DELIMITER ;




DELIMITER //
CREATE TRIGGER TRG_TakipBildirim
AFTER INSERT ON takipler
FOR EACH ROW
BEGIN
    
    IF NEW.takip_eden_id <> NEW.takip_edilen_id THEN
        CALL get_bildirimekle(NEW.takip_eden_id, NEW.takip_edilen_id, NULL, 'takip', NULL);
    END IF;
END //
DELIMITER ;




