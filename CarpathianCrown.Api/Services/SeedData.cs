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
                    DescriptionUa = "Затишний одномісний номер для короткого або ділового перебування.",
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
    DescriptionUa = "Комфортний двомісний номер з м’яким освітленням та сучасним інтер’єром.",
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
    DescriptionUa = "Номер з двома окремими ліжками для друзів або колег.",
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
    DescriptionUa = "Покращений номер з більшою площею та елегантним дизайном.",
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
    DescriptionUa = "Просторий номер з двома ліжками та покращеним комфортом.",
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
    DescriptionUa = "Deluxe номер з преміальним оздобленням та зоною відпочинку.",
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
    DescriptionUa = "Номер з видом на гори та панорамними вікнами.",
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
    DescriptionUa = "Напівлюкс із додатковою зоною відпочинку та покращеним простором.",
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
    DescriptionUa = "Сімейний номер для комфортного проживання з дітьми.",
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
    DescriptionUa = "Просторий сімейний люкс із окремими зонами для сну та відпочинку.",
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
    DescriptionUa = "Представницький люкс для гостей, які цінують статус і простір.",
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
    DescriptionUa = "Найпросторіший люкс із преміальним дизайном та максимальним комфортом.",
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
                    NameUa = "Сніданок у ресторані",
                    NameEn = "Restaurant Breakfast",
                    DescriptionUa = "Розширений сніданок шведської лінії з гарячими стравами, десертами та напоями.",
                    DescriptionEn = "Extended buffet breakfast with hot dishes, desserts, and drinks.",
                    Price = 450,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1552566626-52f8b828add9"
                },
                new ServiceItem
                {
                    Category = "Restaurant",
                    NameUa = "Вечеря в ресторані",
                    NameEn = "Restaurant Dinner",
                    DescriptionUa = "Фіксований сет вечері в авторському ресторані готелю.",
                    DescriptionEn = "Fixed dinner set in the hotel’s signature restaurant.",
                    Price = 950,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1414235077428-338989a2e8c0"
                },
                new ServiceItem
                {
                    Category = "Transport",
                    NameUa = "Паркінг",
                    NameEn = "Parking",
                    DescriptionUa = "Охоронюване місце на паркінгу біля готелю.",
                    DescriptionEn = "Secured parking place near the hotel.",
                    Price = 250,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1506521781263-d8422e82f27a"
                },
                new ServiceItem
                {
                    Category = "Care",
                    NameUa = "Пральня",
                    NameEn = "Laundry",
                    DescriptionUa = "Професійне прання та базовий догляд за одягом.",
                    DescriptionEn = "Professional laundry service and basic garment care.",
                    Price = 400,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1517677208171-0bc6725a3e60"
                },
                new ServiceItem
                {
                    Category = "Care",
                    NameUa = "Хімчистка",
                    NameEn = "Dry Cleaning",
                    DescriptionUa = "Делікатна хімчистка для ділового та святкового одягу.",
                    DescriptionEn = "Delicate dry cleaning for business and formal clothing.",
                    Price = 650,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1517677208171-0bc6725a3e60"
                },
                new ServiceItem
                {
                    Category = "Wellness",
                    NameUa = "Басейн",
                    NameEn = "Swimming Pool",
                    DescriptionUa = "Разовий доступ до басейну готелю з рушниками та зоною відпочинку.",
                    DescriptionEn = "One-time access to the hotel swimming pool with towels and lounge area.",
                    Price = 500,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1575429198097-0414ec08e8cd"
                },
                new ServiceItem
                {
                    Category = "Wellness",
                    NameUa = "Сауна",
                    NameEn = "Sauna",
                    DescriptionUa = "Окремий доступ до сауни та wellness-зони.",
                    DescriptionEn = "Private access to sauna and wellness zone.",
                    Price = 700,
                    IsActive = true,
                    ImageUrl = "https://images.unsplash.com/photo-1519823551278-64ac92734fb1"
                },
                new ServiceItem
                {
                    Category = "Transport",
                    NameUa = "Трансфер з вокзалу",
                    NameEn = "Railway Station Transfer",
                    DescriptionUa = "Індивідуальний трансфер до або з вокзалу.",
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
                TitleUa = "Про нас",
                TitleEn = "About",
                HeroImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4",
                HtmlUa = """
                    <section>
                        <p><b>Sources in Bukovina</b> — сучасний преміальний готель у Чернівцях, створений для гостей, які цінують комфорт, сервіс і атмосферу спокою.</p>
                        <p>Готель поєднує стильні номери, авторський ресторан, wellness-зону та високий рівень гостинності.</p>
                        <ul>
                            <li>12 номерів різних категорій</li>
                            <li>Ресторан української та європейської кухні</li>
                            <li>Басейн, сауна, паркінг, пральня</li>
                            <li>Цілодобова рецепція та швидкий Wi-Fi</li>
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
                TitleUa = "Ресторан",
                TitleEn = "Restaurant",
                HeroImageUrl = "https://images.unsplash.com/photo-1529692236671-f1f6cf9683ba",
                HtmlUa = """
                    <section>
                        <p>Ресторан Sources in Bukovina пропонує страви української та європейської кухні у сучасному авторському виконанні.</p>
                        <p>Для гостей доступні сніданки, бізнес-ланчі та вечірнє меню з винною картою.</p>
                        <ul>
                            <li>Сніданки щодня з 07:00 до 10:30</li>
                            <li>Обіди та вечері по меню</li>
                            <li>Сезонні локальні продукти</li>
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
                TitleUa = "Сервіси",
                TitleEn = "Services",
                HeroImageUrl = "https://images.unsplash.com/photo-1551887373-6c5bd05a07bb",
                HtmlUa = """
                    <section>
                        <p>Готель пропонує додаткові сервіси для максимального комфорту гостей.</p>
                        <ul>
                            <li>Паркінг</li>
                            <li>Пральня та хімчистка</li>
                            <li>Басейн і сауна</li>
                            <li>Індивідуальний трансфер</li>
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
                TitleUa = "Контакти",
                TitleEn = "Contacts",
                HeroImageUrl = "https://images.unsplash.com/photo-1484154218962-a197022b5858",
                HtmlUa = """
                    <section>
                        <p><b>Адреса:</b> м. Чернівці, Україна</p>
                        <p><b>Телефон:</b> +380 66 381 03 58</p>
                        <p><b>Email:</b> info@sourcesbukovina.com</p>
                        <p><b>Рецепція:</b> 24/7</p>
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