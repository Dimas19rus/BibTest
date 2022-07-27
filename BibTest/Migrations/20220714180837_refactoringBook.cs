using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibTest.Migrations
{
    public partial class refactoringBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PathBookCover = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PersonalDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_PersonalData_PersonalDataId",
                        column: x => x.PersonalDataId,
                        principalTable: "PersonalData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoanOfBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsReturned = table.Column<bool>(type: "bit", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanOfBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanOfBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoanOfBooks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name", "Patronymic", "Surname" },
                values: new object[,]
                {
                    { 1, "Александр", "Сергеевич", "Пушкин" },
                    { 2, "Фёдор", "Михайлович", "Достоевский" },
                    { 3, "Николай", "Васильевич", "Гоголь" }
                });

            migrationBuilder.InsertData(
                table: "PersonalData",
                columns: new[] { "Id", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, "Александр", "Пушкин" },
                    { 2, "Фёдор", "Достоевский" },
                    { 3, "Николай", "Гоголь" },
                    { 4, "Галя", "Петровна" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Библиотекарь" },
                    { 2, "Читатель" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Description", "PathBookCover", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Роман в стихах охватывает события с 1819 по 1825 год: от Заграничных походов Русской армии после разгрома Наполеона до восстания декабристов. Сюжет романа прост, в центре повествования — история любви Евгения Онегина и Татьяны Лариной.\r\n\r\n«Евгений Онегин» стал поистине энциклопедией русской жизни времен царствования Александра I, так как широта охваченных в нем тем, детализация быта, многосюжетность композиции, глубина описания характеров персонажей и сейчас достоверно демонстрируют читателям особенности жизни аристократии той эпохи. Одна из главных особенностей романа, отмеченная самим автором, — «разомкнутость» повествования во времени: каждая глава могла бы стать последней, но может иметь и продолжение, то есть каждая глава фактически является самостоятельным и цельным произведением.", "1.jpg", "Евгений Онегин" },
                    { 2, 1, "«Метель» — последнее произведение из цикла «Повести покойного Ивана Петровича Белкина» Александра Пушкина. Развивая мотив судьбы и предопределения в жизни человека, автор в повести «Метель» создает образы заурядных людей и показывает, как причудливо может сложиться жизнь, если вмешается необъяснимый рок. Герои повести — сентиментальная барышня Маша и бедный прапорщик Владимир — влюблены друг в друга. Родители девушки препятствуют браку, и молодые люди решают бежать и тайно венчаться. Все спланировано и рассчитано: верные слуги готовы помочь, друзья жениха соглашаются стать свидетелями, батюшка готов венчать. Но судьба рассудила по-своему. Поднялась метель, закружила в поле жениха, и он опоздал на собственную свадьбу. И эта же метель привела к деревенской церкви проезжего офицера Бурмина, который скуки ради и оказался под венцом с незнакомой барышней.\r\n\r\nРаботу над повестью автор закончил 20 октября 1830 года в Болдино, в 1831 году она была издана.", "2.jpg", "Метель" },
                    { 3, 1, "Героиня повести Лиза — единственное и балованное дитя помещика Муромского; в доме ей все позволено, отец не чает в ней души, и поминутные проказы дочери лишь восхищают его. Деревенская жизнь, чтение европейских романов, дружба с дворовыми крестьянами развивают в ней живость и естественность характера, простоту и искренность поведения, веселость и озорство, мечтательность, жажду любви и счастья. Поэтому роли, в которых она себя пробует — уездная барышня, жеманница-англоманка и крестьянка Акулина, — даются ей легко и свободно.", "3.jpg", "Барышня-крестьянка" },
                    { 4, 2, "Главная идея романа — изобразить положительно прекрасного человека, обладающего нравственной чистотой, благородством, деликатностью, способностью сострадать чужой боли до полного самоотречения. «Прекрасное есть идеал, а идеал — ни наш, ни цивилизованной Европы — еще далеко не выработался», — писал о своей задаче Достоевский. Разрушение социального уклада в России конца 60-х годов XIX века писатель доносит через психологические портреты действующих лиц, через крах семейных и межчеловеческих взаимоотношений, связанных с проблемами власти денег над людьми, безбожия и разгула эгоистических страстей.\r\n\r\nРоман был начат в 1867 году в Женеве, закончен в 1869 году во Флоренции. Публиковался частями в 1868–1869 годах в журнале «Русский вестник».", "4.jpg", "Идиот" },
                    { 5, 2, "Главная идея романа — изобразить положительно прекрасного человека, обладающего нравственной чистотой, благородством, деликатностью, способностью сострадать чужой боли до полного самоотречения. «Прекрасное есть идеал, а идеал — ни наш, ни цивилизованной Европы — еще далеко не выработался», — писал о своей задаче Достоевский. Разрушение социального уклада в России конца 60-х годов XIX века писатель доносит через психологические портреты действующих лиц, через крах семейных и межчеловеческих взаимоотношений, связанных с проблемами власти денег над людьми, безбожия и разгула эгоистических страстей.\r\n\r\nРоман был начат в 1867 году в Женеве, закончен в 1869 году во Флоренции. Публиковался частями в 1868–1869 годах в журнале «Русский вестник».", "5.jpg", "Преступление и наказание" },
                    { 6, 2, "Святочный рассказ «Мальчик у Христа на елке» Федора Достоевского впервые вышел в январе 1875 года на страницах январского выпуска «Дневника писателя». История рассказывает о бедном голодном мальчике, который остался один под Рождество. Он мечтает о празднике, который может видеть лишь в окне господского дома, и в конце концов попадает на Христову елку.", "6.jpg", "Мальчик у Христа на ёлке" },
                    { 7, 3, "«Ревизор» — комедия в пяти действиях. Традиционно считается, что сюжет был подсказан Николаю Гоголю А.С. Пушкиным. Премьера «Ревизора» состоялась 19 апреля 1836 года на сцене Александринского театра в Петербурге. Присутствовал сам император, Николай I. Гоголь был удручен увиденным: замысел комедии, в которой вопреки драматургическим канонам того времени фактически не было положительного героя, не был понят ни актерами, ни зрителями. Из глубокого смысла, вложенного в пьесу, не было ничего извлечено. Комедию приняли за обыкновенный водевиль…", "7.jpg", "Ревизор" },
                    { 8, 3, "«Мертвые души» — произведение русского прозаика, драматурга, поэта, критика, публициста Николая Васильевича Гоголя (1809-1852), которое сам автор обозначил как «поэма». Изначально задумано как трёхтомное произведение. Первый том под названием «Похождения Чичикова, или Мёртвые души» был издан в 1842 году. Практически готовый второй том уничтожен писателем в 1845 году, но сохранилось несколько глав в черновиках, которые были изданы в составе Полного собрания сочинений Гоголя в 1855 году. Третий том был задуман и не начат, о нём остались только обрывочные сведения.\r\n\r\nК этому своему произведению Гоголь относился как к «священному завещанию поэта» и литературному подвигу, имеющему одновременно значение патриотического, «долженствующего открыть судьбы России и мира». Писатель живописует Россию как страну, глубоко пораженную пороками и коррупцией, но именно эта нищета и греховность составляют метафизическую подоплеку ее возрождения.", "8.jpg", "Мёртвые души" },
                    { 9, 3, "В нарочито косноязычной сказовой манере, перебиваемой патетическими монологами, автор повествует о нелепой жизни и смерти «маленького человека», по своему сопротивляющегося обстоятельствам и бунтующего против социальной иерархии. Неожиданное соединение элементов критического реализма и мистического гротеска в этой повести неоднократно ставило в тупик литературоведов и критиков.\r\n\r\nПовесть увидела свет в 3-м томе Собрания сочинений Гоголя, отпечатанного на исходе 1842 года и поступившего в продажу в январе 1843 года. Вошла в историю русской литературы как «манифест социального равенства и неотъемлемых прав личности в любом ее состоянии и звании».", "9.jpg", "Шинель" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "PersonalDataId", "RoleId" },
                values: new object[,]
                {
                    { 1, "Pushkin", "Pushkin", 1, 2 },
                    { 2, "Dostoevski", "Dostoevski", 2, 2 },
                    { 3, "Gogol", "Gogol", 3, 2 },
                    { 4, "Gala", "Gala", 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfBooks_BookId",
                table: "LoanOfBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanOfBooks_UserId",
                table: "LoanOfBooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonalDataId",
                table: "Users",
                column: "PersonalDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanOfBooks");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "PersonalData");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
