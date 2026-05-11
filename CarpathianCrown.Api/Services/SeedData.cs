using CarpathianCrown.Api.Data;
using CarpathianCrown.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarpathianCrown.Api.Services;

public static class SeedData
{
    public static async Task Ensure(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync(u => u.Role == "Admin"))
        {
            db.Users.Add(new User
            {
                Email = "admin@carpathiancrown.com",
                Login = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = "Admin",
                FirstName = "System",
                LastName = "Admin",
                Phone = "+380663810358"
            });
        }

        var hasBookings = await db.Bookings.AnyAsync();

        if (!hasBookings)
        {
            if (await db.Rooms.AnyAsync())
                db.Rooms.RemoveRange(await db.Rooms.ToListAsync());

            if (await db.ServiceItems.AnyAsync())
                db.ServiceItems.RemoveRange(await db.ServiceItems.ToListAsync());
        }

        if (await db.ContentPages.AnyAsync())
            db.ContentPages.RemoveRange(await db.ContentPages.ToListAsync());

        await db.SaveChangesAsync();

        if (!hasBookings)
        {
            db.Rooms.AddRange(
                new Room
                {
                    NameUa = "Standard Single",
                    NameEn = "Standard Single",
                    DescriptionUa = "Р—Р°С‚РёС€РЅРёР№ РѕРґРЅРѕРјС–СЃРЅРёР№ РЅРѕРјРµСЂ РґР»СЏ РєРѕСЂРѕС‚РєРѕРіРѕ Р°Р±Рѕ РґС–Р»РѕРІРѕРіРѕ РїРµСЂРµР±СѓРІР°РЅРЅСЏ.",
                    DescriptionEn = "A cozy single room for short or business stays.",
                    PricePerNight = 2200,
                    Capacity = 1,
                    Status = "Available",
                    CoverImageUrl = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85",
                    Image2 = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267",
                    Image3 = "https://images.unsplash.com/photo-1501117716987-c8e1ecb210d8",
                    Image4 = "https://images.unsplash.com/photo-1560185007-c5ca9d2c014d"
                },
new Room
{
    NameUa = "Standard Double",
    NameEn = "Standard Double",
    DescriptionUa = "РљРѕРјС„РѕСЂС‚РЅРёР№ РґРІРѕРјС–СЃРЅРёР№ РЅРѕРјРµСЂ Р· РјвЂ™СЏРєРёРј РѕСЃРІС–С‚Р»РµРЅРЅСЏРј С‚Р° СЃСѓС‡Р°СЃРЅРёРј С–РЅС‚РµСЂвЂ™С”СЂРѕРј.",
    DescriptionEn = "Comfortable double room with soft lighting and modern interior.",
    PricePerNight = 2800,
    Capacity = 2,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267",
    Image2 = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85",
    Image3 = "https://images.unsplash.com/photo-1560185007-c5ca9d2c014d",
    Image4 = "https://images.unsplash.com/photo-1590490360182-c33d57733427"
},
new Room
{
    NameUa = "Standard Twin",
    NameEn = "Standard Twin",
    DescriptionUa = "РќРѕРјРµСЂ Р· РґРІРѕРјР° РѕРєСЂРµРјРёРјРё Р»С–Р¶РєР°РјРё РґР»СЏ РґСЂСѓР·С–РІ Р°Р±Рѕ РєРѕР»РµРі.",
    DescriptionEn = "A room with two separate beds for friends or colleagues.",
    PricePerNight = 2900,
    Capacity = 2,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1501117716987-c8e1ecb210d8",
    Image2 = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267",
    Image3 = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85",
    Image4 = "https://images.unsplash.com/photo-1566665797739-1674de7a421a"
},
new Room
{
    NameUa = "Superior Double",
    NameEn = "Superior Double",
    DescriptionUa = "РџРѕРєСЂР°С‰РµРЅРёР№ РЅРѕРјРµСЂ Р· Р±С–Р»СЊС€РѕСЋ РїР»РѕС‰РµСЋ С‚Р° РµР»РµРіР°РЅС‚РЅРёРј РґРёР·Р°Р№РЅРѕРј.",
    DescriptionEn = "Superior room with extra space and elegant design.",
    PricePerNight = 3400,
    Capacity = 2,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1560185007-c5ca9d2c014d",
    Image2 = "https://images.unsplash.com/photo-1566665797739-1674de7a421a",
    Image3 = "https://images.unsplash.com/photo-1455587734955-081b22074882",
    Image4 = "https://images.unsplash.com/photo-1590490360182-c33d57733427"
},
new Room
{
    NameUa = "Superior Twin",
    NameEn = "Superior Twin",
    DescriptionUa = "РџСЂРѕСЃС‚РѕСЂРёР№ РЅРѕРјРµСЂ Р· РґРІРѕРјР° Р»С–Р¶РєР°РјРё С‚Р° РїРѕРєСЂР°С‰РµРЅРёРј РєРѕРјС„РѕСЂС‚РѕРј.",
    DescriptionEn = "Spacious twin room with improved comfort.",
    PricePerNight = 3500,
    Capacity = 2,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1566665797739-1674de7a421a",
    Image2 = "https://images.unsplash.com/photo-1560185007-c5ca9d2c014d",
    Image3 = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85",
    Image4 = "https://images.unsplash.com/photo-1455587734955-081b22074882"
},
new Room
{
    NameUa = "Deluxe Double",
    NameEn = "Deluxe Double",
    DescriptionUa = "Deluxe РЅРѕРјРµСЂ Р· РїСЂРµРјС–Р°Р»СЊРЅРёРј РѕР·РґРѕР±Р»РµРЅРЅСЏРј С‚Р° Р·РѕРЅРѕСЋ РІС–РґРїРѕС‡РёРЅРєСѓ.",
    DescriptionEn = "Deluxe room with premium finishing and lounge area.",
    PricePerNight = 4200,
    Capacity = 2,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1455587734955-081b22074882",
    Image2 = "https://images.unsplash.com/photo-1505692952047-1a78307da8f2",
    Image3 = "https://images.unsplash.com/photo-1590490360182-c33d57733427",
    Image4 = "https://images.unsplash.com/photo-1578898887932-dce23a595ad4"
},
new Room
{
    NameUa = "Deluxe Mountain View",
    NameEn = "Deluxe Mountain View",
    DescriptionUa = "РќРѕРјРµСЂ Р· РІРёРґРѕРј РЅР° РіРѕСЂРё С‚Р° РїР°РЅРѕСЂР°РјРЅРёРјРё РІС–РєРЅР°РјРё.",
    DescriptionEn = "A room with mountain view and panoramic windows.",
    PricePerNight = 4800,
    Capacity = 2,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1505692952047-1a78307da8f2",
    Image2 = "https://images.unsplash.com/photo-1455587734955-081b22074882",
    Image3 = "https://images.unsplash.com/photo-1590490360182-c33d57733427",
    Image4 = "https://images.unsplash.com/photo-1540518614846-7eded433c457"
},
new Room
{
    NameUa = "Junior Suite",
    NameEn = "Junior Suite",
    DescriptionUa = "РќР°РїС–РІР»СЋРєСЃ С–Р· РґРѕРґР°С‚РєРѕРІРѕСЋ Р·РѕРЅРѕСЋ РІС–РґРїРѕС‡РёРЅРєСѓ С‚Р° РїРѕРєСЂР°С‰РµРЅРёРј РїСЂРѕСЃС‚РѕСЂРѕРј.",
    DescriptionEn = "Junior suite with extra lounge zone and extended space.",
    PricePerNight = 5600,
    Capacity = 3,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1590490360182-c33d57733427",
    Image2 = "https://images.unsplash.com/photo-1578683010236-d716f9a3f461",
    Image3 = "https://images.unsplash.com/photo-1582582621959-48d27397dc69",
    Image4 = "https://images.unsplash.com/photo-1578898887932-dce23a595ad4"
},
new Room
{
    NameUa = "Family Room",
    NameEn = "Family Room",
    DescriptionUa = "РЎС–РјРµР№РЅРёР№ РЅРѕРјРµСЂ РґР»СЏ РєРѕРјС„РѕСЂС‚РЅРѕРіРѕ РїСЂРѕР¶РёРІР°РЅРЅСЏ Р· РґС–С‚СЊРјРё.",
    DescriptionEn = "Family room for comfortable stay with children.",
    PricePerNight = 6200,
    Capacity = 4,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1578683010236-d716f9a3f461",
    Image2 = "https://images.unsplash.com/photo-1582582621959-48d27397dc69",
    Image3 = "https://images.unsplash.com/photo-1590490360182-c33d57733427",
    Image4 = "https://images.unsplash.com/photo-1540518614846-7eded433c457"
},
new Room
{
    NameUa = "Family Suite",
    NameEn = "Family Suite",
    DescriptionUa = "РџСЂРѕСЃС‚РѕСЂРёР№ СЃС–РјРµР№РЅРёР№ Р»СЋРєСЃ С–Р· РѕРєСЂРµРјРёРјРё Р·РѕРЅР°РјРё РґР»СЏ СЃРЅСѓ С‚Р° РІС–РґРїРѕС‡РёРЅРєСѓ.",
    DescriptionEn = "Spacious family suite with separate sleep and lounge areas.",
    PricePerNight = 7600,
    Capacity = 4,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1582582621959-48d27397dc69",
    Image2 = "https://images.unsplash.com/photo-1578683010236-d716f9a3f461",
    Image3 = "https://images.unsplash.com/photo-1578898887932-dce23a595ad4",
    Image4 = "https://images.unsplash.com/photo-1540518614846-7eded433c457"
},
new Room
{
    NameUa = "Executive Suite",
    NameEn = "Executive Suite",
    DescriptionUa = "РџСЂРµРґСЃС‚Р°РІРЅРёС†СЊРєРёР№ Р»СЋРєСЃ РґР»СЏ РіРѕСЃС‚РµР№, СЏРєС– С†С–РЅСѓСЋС‚СЊ СЃС‚Р°С‚СѓСЃ С– РїСЂРѕСЃС‚С–СЂ.",
    DescriptionEn = "Executive suite for guests who value prestige and space.",
    PricePerNight = 9800,
    Capacity = 4,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1578898887932-dce23a595ad4",
    Image2 = "https://images.unsplash.com/photo-1540518614846-7eded433c457",
    Image3 = "https://images.unsplash.com/photo-1582582621959-48d27397dc69",
    Image4 = "https://images.unsplash.com/photo-1590490360182-c33d57733427"
},
new Room
{
    NameUa = "Presidential Suite",
    NameEn = "Presidential Suite",
    DescriptionUa = "РќР°Р№РїСЂРѕСЃС‚РѕСЂС–С€РёР№ Р»СЋРєСЃ С–Р· РїСЂРµРјС–Р°Р»СЊРЅРёРј РґРёР·Р°Р№РЅРѕРј С‚Р° РјР°РєСЃРёРјР°Р»СЊРЅРёРј РєРѕРјС„РѕСЂС‚РѕРј.",
    DescriptionEn = "The most spacious suite with premium design and maximum comfort.",
    PricePerNight = 14500,
    Capacity = 4,
    Status = "Available",
    CoverImageUrl = "https://images.unsplash.com/photo-1540518614846-7eded433c457",
    Image2 = "https://images.unsplash.com/photo-1578898887932-dce23a595ad4",
    Image3 = "https://images.unsplash.com/photo-1582582621959-48d27397dc69",
    Image4 = "https://images.unsplash.com/photo-1590490360182-c33d57733427"
}
            );

            db.ServiceItems.AddRange(
                new ServiceItem
                {
                    Category = "Restaurant",
                    NameUa = "РЎРЅС–РґР°РЅРѕРє Сѓ СЂРµСЃС‚РѕСЂР°РЅС–",
                    NameEn = "Restaurant Breakfast",
                    DescriptionUa = "Р РѕР·С€РёСЂРµРЅРёР№ СЃРЅС–РґР°РЅРѕРє С€РІРµРґСЃСЊРєРѕС— Р»С–РЅС–С— Р· РіР°СЂСЏС‡РёРјРё СЃС‚СЂР°РІР°РјРё, РґРµСЃРµСЂС‚Р°РјРё С‚Р° РЅР°РїРѕСЏРјРё.",
                    DescriptionEn = "Extended buffet breakfast with hot dishes, desserts, and drinks.",
                    Price = 450,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1552566626-52f8b828add9"
                },
                new ServiceItem
                {
                    Category = "Restaurant",
                    NameUa = "Р’РµС‡РµСЂСЏ РІ СЂРµСЃС‚РѕСЂР°РЅС–",
                    NameEn = "Restaurant Dinner",
                    DescriptionUa = "Р¤С–РєСЃРѕРІР°РЅРёР№ СЃРµС‚ РІРµС‡РµСЂС– РІ Р°РІС‚РѕСЂСЃСЊРєРѕРјСѓ СЂРµСЃС‚РѕСЂР°РЅС– РіРѕС‚РµР»СЋ.",
                    DescriptionEn = "Fixed dinner set in the hotelвЂ™s signature restaurant.",
                    Price = 950,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1414235077428-338989a2e8c0"
                },
                new ServiceItem
                {
                    Category = "Transport",
                    NameUa = "РџР°СЂРєС–РЅРі",
                    NameEn = "Parking",
                    DescriptionUa = "РћС…РѕСЂРѕРЅСЋРІР°РЅРµ РјС–СЃС†Рµ РЅР° РїР°СЂРєС–РЅРіСѓ Р±С–Р»СЏ РіРѕС‚РµР»СЋ.",
                    DescriptionEn = "Secured parking place near the hotel.",
                    Price = 250,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1506521781263-d8422e82f27a"
                },
                new ServiceItem
                {
                    Category = "Care",
                    NameUa = "РџСЂР°Р»СЊРЅСЏ",
                    NameEn = "Laundry",
                    DescriptionUa = "РџСЂРѕС„РµСЃС–Р№РЅРµ РїСЂР°РЅРЅСЏ С‚Р° Р±Р°Р·РѕРІРёР№ РґРѕРіР»СЏРґ Р·Р° РѕРґСЏРіРѕРј.",
                    DescriptionEn = "Professional laundry service and basic garment care.",
                    Price = 400,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1517677208171-0bc6725a3e60"
                },
                new ServiceItem
                {
                    Category = "Care",
                    NameUa = "РҐС–РјС‡РёСЃС‚РєР°",
                    NameEn = "Dry Cleaning",
                    DescriptionUa = "Р”РµР»С–РєР°С‚РЅР° С…С–РјС‡РёСЃС‚РєР° РґР»СЏ РґС–Р»РѕРІРѕРіРѕ С‚Р° СЃРІСЏС‚РєРѕРІРѕРіРѕ РѕРґСЏРіСѓ.",
                    DescriptionEn = "Delicate dry cleaning for business and formal clothing.",
                    Price = 650,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1517677208171-0bc6725a3e60"
                },
                new ServiceItem
                {
                    Category = "Wellness",
                    NameUa = "Р‘Р°СЃРµР№РЅ",
                    NameEn = "Swimming Pool",
                    DescriptionUa = "Р Р°Р·РѕРІРёР№ РґРѕСЃС‚СѓРї РґРѕ Р±Р°СЃРµР№РЅСѓ РіРѕС‚РµР»СЋ Р· СЂСѓС€РЅРёРєР°РјРё С‚Р° Р·РѕРЅРѕСЋ РІС–РґРїРѕС‡РёРЅРєСѓ.",
                    DescriptionEn = "One-time access to the hotel swimming pool with towels and lounge area.",
                    Price = 500,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1575429198097-0414ec08e8cd"
                },
                new ServiceItem
                {
                    Category = "Wellness",
                    NameUa = "РЎР°СѓРЅР°",
                    NameEn = "Sauna",
                    DescriptionUa = "РћРєСЂРµРјРёР№ РґРѕСЃС‚СѓРї РґРѕ СЃР°СѓРЅРё С‚Р° wellness-Р·РѕРЅРё.",
                    DescriptionEn = "Private access to sauna and wellness zone.",
                    Price = 700,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1519823551278-64ac92734fb1"
                },
                new ServiceItem
                {
                    Category = "Transport",
                    NameUa = "РўСЂР°РЅСЃС„РµСЂ Р· РІРѕРєР·Р°Р»Сѓ",
                    NameEn = "Railway Station Transfer",
                    DescriptionUa = "Р†РЅРґРёРІС–РґСѓР°Р»СЊРЅРёР№ С‚СЂР°РЅСЃС„РµСЂ РґРѕ Р°Р±Рѕ Р· РІРѕРєР·Р°Р»Сѓ.",
                    DescriptionEn = "Private transfer to or from the railway station.",
                    Price = 900,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1449965408869-eaa3f722e40d"
                }
            );
        }

        db.ContentPages.AddRange(
            new ContentPage
            {
                Slug = "about",
                TitleUa = "РџСЂРѕ РЅР°СЃ",
                TitleEn = "About",
                HeroImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4",
                HtmlUa = """
                    <section>
                        <p><b>Sources in Bukovina</b> вЂ” СЃСѓС‡Р°СЃРЅРёР№ РїСЂРµРјС–Р°Р»СЊРЅРёР№ РіРѕС‚РµР»СЊ Сѓ Р§РµСЂРЅС–РІС†СЏС…, СЃС‚РІРѕСЂРµРЅРёР№ РґР»СЏ РіРѕСЃС‚РµР№, СЏРєС– С†С–РЅСѓСЋС‚СЊ РєРѕРјС„РѕСЂС‚, СЃРµСЂРІС–СЃ С– Р°С‚РјРѕСЃС„РµСЂСѓ СЃРїРѕРєРѕСЋ.</p>
                        <p>Р“РѕС‚РµР»СЊ РїРѕС”РґРЅСѓС” СЃС‚РёР»СЊРЅС– РЅРѕРјРµСЂРё, Р°РІС‚РѕСЂСЃСЊРєРёР№ СЂРµСЃС‚РѕСЂР°РЅ, wellness-Р·РѕРЅСѓ С‚Р° РІРёСЃРѕРєРёР№ СЂС–РІРµРЅСЊ РіРѕСЃС‚РёРЅРЅРѕСЃС‚С–.</p>
                        <ul>
                            <li>12 РЅРѕРјРµСЂС–РІ СЂС–Р·РЅРёС… РєР°С‚РµРіРѕСЂС–Р№</li>
                            <li>Р РµСЃС‚РѕСЂР°РЅ СѓРєСЂР°С—РЅСЃСЊРєРѕС— С‚Р° С”РІСЂРѕРїРµР№СЃСЊРєРѕС— РєСѓС…РЅС–</li>
                            <li>Р‘Р°СЃРµР№РЅ, СЃР°СѓРЅР°, РїР°СЂРєС–РЅРі, РїСЂР°Р»СЊРЅСЏ</li>
                            <li>Р¦С–Р»РѕРґРѕР±РѕРІР° СЂРµС†РµРїС†С–СЏ С‚Р° С€РІРёРґРєРёР№ Wi-Fi</li>
                        </ul>
                    </section>
                    """,
                HtmlEn = """
                    <section>
                        <p><b>Sources in Bukovina</b> is a modern premium hotel in Chernivtsi created for guests who value comfort, service, and a calm atmosphere.</p>
                        <p>The hotel combines stylish rooms, a signature restaurant, a wellness area, and a high level of hospitality.</p>
                        <ul>
                            <li>12 rooms of different categories</li>
                            <li>Restaurant with Ukrainian and European cuisine</li>
                            <li>Pool, sauna, parking, laundry</li>
                            <li>24/7 reception and fast Wi-Fi</li>
                        </ul>
                    </section>
                    """
            },
            new ContentPage
            {
                Slug = "restaurant",
                TitleUa = "Р РµСЃС‚РѕСЂР°РЅ",
                TitleEn = "Restaurant",
                HeroImageUrl = "https://images.unsplash.com/photo-1529692236671-f1f6cf9683ba",
                HtmlUa = """
                    <section>
                        <p>Р РµСЃС‚РѕСЂР°РЅ Sources in Bukovina РїСЂРѕРїРѕРЅСѓС” СЃС‚СЂР°РІРё СѓРєСЂР°С—РЅСЃСЊРєРѕС— С‚Р° С”РІСЂРѕРїРµР№СЃСЊРєРѕС— РєСѓС…РЅС– Сѓ СЃСѓС‡Р°СЃРЅРѕРјСѓ Р°РІС‚РѕСЂСЃСЊРєРѕРјСѓ РІРёРєРѕРЅР°РЅРЅС–.</p>
                        <p>Р”Р»СЏ РіРѕСЃС‚РµР№ РґРѕСЃС‚СѓРїРЅС– СЃРЅС–РґР°РЅРєРё, Р±С–Р·РЅРµСЃ-Р»Р°РЅС‡С– С‚Р° РІРµС‡С–СЂРЅС” РјРµРЅСЋ Р· РІРёРЅРЅРѕСЋ РєР°СЂС‚РѕСЋ.</p>
                        <ul>
                            <li>РЎРЅС–РґР°РЅРєРё С‰РѕРґРЅСЏ Р· 07:00 РґРѕ 10:30</li>
                            <li>РћР±С–РґРё С‚Р° РІРµС‡РµСЂС– РїРѕ РјРµРЅСЋ</li>
                            <li>РЎРµР·РѕРЅРЅС– Р»РѕРєР°Р»СЊРЅС– РїСЂРѕРґСѓРєС‚Рё</li>
                        </ul>
                    </section>
                    """,
                HtmlEn = """
                    <section>
                        <p>The Sources in Bukovina restaurant offers Ukrainian and European cuisine in a refined contemporary interpretation.</p>
                        <p>Guests can enjoy breakfast, business lunches, and an evening menu with a wine selection.</p>
                        <ul>
                            <li>Breakfast daily from 07:00 to 10:30</li>
                            <li>Lunches and dinners from the menu</li>
                            <li>Seasonal local ingredients</li>
                        </ul>
                    </section>
                    """
            },
            new ContentPage
            {
                Slug = "services",
                TitleUa = "РЎРµСЂРІС–СЃРё",
                TitleEn = "Services",
                HeroImageUrl = "https://images.unsplash.com/photo-1551887373-6c5bd05a07bb",
                HtmlUa = """
                    <section>
                        <p>Р“РѕС‚РµР»СЊ РїСЂРѕРїРѕРЅСѓС” РґРѕРґР°С‚РєРѕРІС– СЃРµСЂРІС–СЃРё РґР»СЏ РјР°РєСЃРёРјР°Р»СЊРЅРѕРіРѕ РєРѕРјС„РѕСЂС‚Сѓ РіРѕСЃС‚РµР№.</p>
                        <ul>
                            <li>РџР°СЂРєС–РЅРі</li>
                            <li>РџСЂР°Р»СЊРЅСЏ С‚Р° С…С–РјС‡РёСЃС‚РєР°</li>
                            <li>Р‘Р°СЃРµР№РЅ С– СЃР°СѓРЅР°</li>
                            <li>Р†РЅРґРёРІС–РґСѓР°Р»СЊРЅРёР№ С‚СЂР°РЅСЃС„РµСЂ</li>
                        </ul>
                    </section>
                    """,
                HtmlEn = """
                    <section>
                        <p>The hotel provides additional services for maximum guest comfort.</p>
                        <ul>
                            <li>Parking</li>
                            <li>Laundry and dry cleaning</li>
                            <li>Pool and sauna</li>
                            <li>Private transfer</li>
                        </ul>
                    </section>
                    """
            },
            new ContentPage
            {
                Slug = "contacts",
                TitleUa = "РљРѕРЅС‚Р°РєС‚Рё",
                TitleEn = "Contacts",
                HeroImageUrl = "https://images.unsplash.com/photo-1484154218962-a197022b5858",
                HtmlUa = """
                    <section>
                        <p><b>РђРґСЂРµСЃР°:</b> Рј. Р§РµСЂРЅС–РІС†С–, РЈРєСЂР°С—РЅР°</p>
                        <p><b>РўРµР»РµС„РѕРЅ:</b> +380 66 381 03 58</p>
                        <p><b>Email:</b> info@sourcesbukovina.com</p>
                        <p><b>Р РµС†РµРїС†С–СЏ:</b> 24/7</p>
                    </section>
                    """,
                HtmlEn = """
                    <section>
                        <p><b>Address:</b> Chernivtsi, Ukraine</p>
                        <p><b>Phone:</b> +380 66 381 03 58</p>
                        <p><b>Email:</b> info@sourcesbukovina.com</p>
                        <p><b>Reception:</b> 24/7</p>
                    </section>
                    """
            }
        );

        await db.SaveChangesAsync();
    }
}
