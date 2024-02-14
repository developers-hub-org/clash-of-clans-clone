namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Info : MonoBehaviour
    {

        [SerializeField] private GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] public TextMeshProUGUI _titleText = null;
        [SerializeField] public TextMeshProUGUI _descriptionText = null;
        [SerializeField] public Image _icon = null;

        private static UI_Info _instance = null; public static UI_Info instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _active = false;
            _elements.SetActive(false);
        }

        public void OpenUnitInfo(Data.UnitID id)
        {
            Sprite icon = AssetsBank.GetUnitIcon(id);
            if (icon != null)
            {
                _icon.sprite = icon;
            }
            _titleText.text = Language.instanse.GetUnitName(id);
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    _descriptionText.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    switch (id)
                    {
                        case Data.UnitID.barbarian:
                            _descriptionText.text = "این واحدها سربازان پیاده تک هدف هستند. آنها اولین سربازانی هستند که برای نبرد آزاد می شوند و بدیهی است که به اندازه دیگران قوی نیستند. با این حال، اگر از استراتژی درستی استفاده کنید، می توانید از آن ها برای انحراف توجه دشمن در حالی که نیروهای دیگرتان حمله می کنند استفاده کنید.";
                            break;
                        case Data.UnitID.archer:
                            _descriptionText.text = "این واحدها سربازان تک واحدی هستند. آنها به یک هدف، هوا یا زمین حمله می کنند. آنها سلامتی کم دارند بنابراین برد آنها بزرگترین مزیت آنها است. این تیراندازان دوست دارند در میدان جنگ فاصله خود را حفظ کنند.";
                            break;
                        case Data.UnitID.goblin:
                            _descriptionText.text = "این واحدها ساختمان های منابع را بیش از همه اهداف دیگر در اولویت قرار می دهند و تا زمانی که ساختمان های منابع در میدان نبرد وجود دارند، همه انواع دیگر ساختمان ها و نیروهای دشمن را نادیده میگیرند.";
                            break;
                        case Data.UnitID.healer:
                            _descriptionText.text = "یک واحد پرنده بدون قابلیت حمله، اما می تواند هر نیروی زمینی را درمان کند.";
                            break;
                        case Data.UnitID.wallbreaker:
                            _descriptionText.text = "این واحد به دنبال دیوارها می رود و با پیدا کردن نزدیک ترین ساختمان محافظت شده و تخریب دیوارهای محافظ آن، آسیب های هنگفتی به آنها وارد می کند و در این فرآیند خود را منفجر می کند.";
                            break;
                        case Data.UnitID.giant:
                            _descriptionText.text = "این نیرو ها ابتدا به سیستم های دفاعی دشمن حمله می کنند که این امر آنها را به یک نیروی ایده آل برای خلاصی سریع از شر دفاع تبدیل می کند. با این حال، به دلیل خسارت کم، آنها بهتر است در گروه های بزرگ استفاده شوند.";
                            break;
                        case Data.UnitID.miner:
                            _descriptionText.text = "این واحدها می توانند راه خود را در زیر زمین حفاری کنند و در هر نقطه از میدان جنگ ظاهر شوند.";
                            break;
                        case Data.UnitID.balloon:
                            _descriptionText.text = "این واحدها قدرت خسارت زیادی دارند، اما دامنه و سرعت حمله پایینی دارند. آنها همچنین نمی توانند به واحدهای هوایی ضربه بزنند. اولویت این نیروها حمله به سیستم های دفاعی دشمن است.";
                            break;
                        case Data.UnitID.wizard:
                            _descriptionText.text = "اینها واحدهای زمینی شکننده با آسیب پاششی زیاد هستند. آنها معمولاً در گروه های بزرگ برای پشتیبانی آتش یا به عنوان یک تقویت کننده استفاده می شوند، اما می توانند در تعداد کمتر نیز موثر باشند.";
                            break;
                        case Data.UnitID.dragon:
                            _descriptionText.text = "آنها واحدهای پرنده ترسناکی هستند که قادر به حمله به واحدهای زمینی و هوایی دشمن میباشند و دارای سلامت و خسارت بالا هستند.";
                            break;
                        case Data.UnitID.pekka:
                            _descriptionText.text = "آنها نیروهای تک هدف و آهسته ای هستند که حجم زیادی از ظرفیت نیروها را اشغال می کنند اما دارای مقادیر زیادی سلامتی و خسارت میباشند.";
                            break;
                        case Data.UnitID.babydragon:
                            _descriptionText.text = "آنها نیروهای غارتگر عالی هستند زیرا خسارت زیادی وارد می کنند و سلامتی مناسبی دارند. آنها می توانند به سرعت منابع خارجی را نابود کنند. با این حال، دفاع ضد هوایی می تواند آنها را نابود کند.";
                            break;
                        default:
                            _descriptionText.text = "";
                            break;
                    }
                    break;
                default:
                    _descriptionText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    switch (id)
                    {
                        case Data.UnitID.barbarian:
                            _descriptionText.text = "These units are single target melee troops. They are the first troops unlocked for battle and obviously not as strong as others. However, if you use the right strategy, you can use them to tank while your other troops attack.";
                            break;
                        case Data.UnitID.archer:
                            _descriptionText.text = "These units are single housing space troops. They attack a single target, either air or ground. They have low HP so their range is their greatest advantage. These sharpshooters like to keep their distance on the battlefield.";
                            break;
                        case Data.UnitID.goblin:
                            _descriptionText.text = "These units prioritize resource buildings above all other targets, and will bypass all other types of enemy buildings and troops while any resource buildings remain on the battlefield.";
                            break;
                        case Data.UnitID.healer:
                            _descriptionText.text = "A flying unit with no attacking capabilities, but can heal any ground troops.";
                            break;
                        case Data.UnitID.wallbreaker:
                            _descriptionText.text = "This unit will go after Walls, dealing massive damage to them and attacks by locating the nearest protected building and destroying its protective Walls, blowing itself up in the process.";
                            break;
                        case Data.UnitID.giant:
                            _descriptionText.text = "These units first target is defenses, making them an ideal troop to deploy to get rid of defenses fast. However, due to their low attack damage they are better used in large groups.";
                            break;
                        case Data.UnitID.miner:
                            _descriptionText.text = "These units can burrow their way underground and appear anywhere in the arena.";
                            break;
                        case Data.UnitID.balloon:
                            _descriptionText.text = "These units have high damage for their housing space, but low attack range and rate. They also can't hit air units. The priority of these units is to attack the enemy's defense systems";
                            break;
                        case Data.UnitID.wizard:
                            _descriptionText.text = "These are fragile ground units with high splash damage. They are commonly used in large groups for fire support or as a force multiplier, but they can also be effective in smaller numbers, especially at lower levels.";
                            break;
                        case Data.UnitID.dragon:
                            _descriptionText.text = "They are fearsome flying units capable of attacking both ground and air units, with both high health and damage.";
                            break;
                        case Data.UnitID.pekka:
                            _descriptionText.text = "They are slow single target melee troops that occupies a big amount of housing space but comes with large amounts of hitpoints and damage.";
                            break;
                        case Data.UnitID.babydragon:
                            _descriptionText.text = "They are excellent farming troops since they deal high damage and have decent health. They can destroy outside collectors fast. However, anti air defenses can cut them down, so it is recommended to have some sort of initial phase to deal with one or two of them.";
                            break;
                        default:
                            _descriptionText.text = "";
                            break;
                    }
                    break;
            }
            _active = true;
            transform.SetAsLastSibling();
            _elements.SetActive(true);
            _titleText.ForceMeshUpdate(true);
            _descriptionText.ForceMeshUpdate(true);
        }

        public void OpenBuildingInfo(Data.BuildingID id, int level)
        {
            Sprite icon = AssetsBank.GetBuildingIcon(id, level);
            if (icon != null)
            {
                _icon.sprite = icon;
            }
            _titleText.text = Language.instanse.GetBuildingName(id, level);
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    _descriptionText.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    switch (id)
                    {
                        case Data.BuildingID.townhall:
                            _descriptionText.text = "این ساختمان اصلی شهرک شماست. ارتقاء آن قفل ساختمان ها، نیروها و جادوهای جدید را باز می کند. حفاظت از این ساختمان حیاتی است و نیروهای دشمن تنها برای از بین بردن آن در نبرد به یک ستاره دست خواهند یافت.";
                            break;
                        case Data.BuildingID.goldmine:
                            _descriptionText.text = "این ساختمان طلا را از یک ذخیره نامحدود زیرزمینی جمع آوری می کند و آن را ذخیره می کند تا زمانی که توسط بازیکن جمع آوری شده و در یک انبار قرار گیرد. وقتی ساختمان پر شد، تولید متوقف می شود تا زمانی که بازیکن آن را جمع آوری کند و یا دشمن آن را مورد حمله قرار دهد.";
                            break;
                        case Data.BuildingID.goldstorage:
                            _descriptionText.text = "تمام طلاهای گرانبهای شما در اینجا ذخیره می شود. این ساختمان به بازیکنان اجازه می دهد تا طلای سخت به دست آمده را ذخیره کنند تا بتوان از آن برای ارتقاء های بعدی استفاده کرد. حفاظت از این ساختمان بسیار مهم است و نیروهای دشمن می توانند طلاهای داخل آن را در حین نبرد بدزدند.";
                            break;
                        case Data.BuildingID.elixirmine:
                            _descriptionText.text = "این ساختمان اکسیر را از یک ذخیره نامحدود زیرزمینی جمع آوری می کند و آن را ذخیره می کند تا زمانی که توسط بازیکن جمع آوری شده و در یک انبار قرار گیرد. وقتی ساختمان پر شد، تولید متوقف می شود تا زمانی که بازیکن آن را جمع آوری کند و یا دشمن آن را مورد حمله قرار دهد.";
                            break;
                        case Data.BuildingID.elixirstorage:
                            _descriptionText.text = "تمام اکسیر های با ارزش شما در اینجا ذخیره می شود. این ساختمان به بازیکنان اجازه می دهد تا اکسیر سخت به دست آمده را ذخیره کنند تا بتوان از آن برای ارتقاء های بعدی استفاده کرد. حفاظت از این ساختمان بسیار مهم است و نیروهای دشمن می توانند اکسیر داخل آن را در حین نبرد بدزدند.";
                            break;
                        case Data.BuildingID.buildershut:
                            _descriptionText.text = "هیچ کاری در اینجا بدون سازندگان انجام نمی شود. هر یک از این ساختمان ها می توانند هر بار فقط یک سازنده را در خود جای دهند. شما نمی توانید چیزی بسازید یا ارتقا دهید مگر اینکه سازندگان آزاد داشته باشید.";
                            break;
                        case Data.BuildingID.armycamp:
                            _descriptionText.text = "نیروهای شما در اینجا مستقر هستند. این ساختمانی است که ظرفیت نیروها را باز می کند. ظرفیت بیشتر نیروها به شما امکان می دهد ارتش بزرگتری داشته باشید.";
                            break;
                        case Data.BuildingID.barracks:
                            _descriptionText.text = "این ساختمان به شما اجازه می دهد تا نیروهایی را برای حمله به دشمنان خود آموزش دهید. آن را ارتقا دهید تا واحدهای پیشرفته ای را باز کنید که می توانند در نبردهای حماسی پیروز شوند. هر سطح خاص قفل یک سرباز جدید را باز می کند. سربازان در صف را می توان در هر زمان لغو کرد اما منابع بازگردانده نمی شوند.";
                            break;
                        case Data.BuildingID.wall:
                            _descriptionText.text = "هدف اصلی دیوارها جلوگیری از حمله نیروهای زمینی است و به نیروهای دفاعی اجازه می‌دهد تا مهاجمان را در هنگام شکستن دیوارها آسیب بزنند و بکشند. هنگامی که دیوار شکسته میشود، نیروهای مهاجم می‌توانند نیروهای دفاعی و دیگر ساختمان‌های داخل دیوار را از بین ببرند.";
                            break;
                        case Data.BuildingID.cannon:
                            _descriptionText.text = "این یک سیستم دفاعی تک هدف است که خسارت متوسطی را وارد می کند. این اولین سیستم دفاعی است که یک بازیکن در شروع ماجراجویی خود می سازد. این ساختمان هم ارزان است و هم به سرعت در سطوح پایین تر ارتقا می یابد.";
                            break;
                        case Data.BuildingID.archertower:
                            _descriptionText.text = "این یک سیستم دفاعی تک هدف در بازی است. آنها دومین سیستم دفاعی در دسترس بازیکنان و اولین دفاع برای حمله به نیروهای هوایی هستند. این ساختمان ها سازه های بسیار متنوعی هستند. آنها قادر به هدف قرار دادن واحدهای زمینی و هوایی هستند و برد بسیار خوبی دارند.";
                            break;
                        case Data.BuildingID.mortor:
                            _descriptionText.text = "این سیستم دفاعی گلوله‌های انفجاری دوربرد شلیک می‌کند که به هر واحد زمینی در شعاع کوچکی از نقطه برخورد آسیب می‌رساند. خسارت پاششی همراه با برد بلند، آنها را به سلاح های مرگبار در برابر گروه های بزرگی از دشمنان ضعیف تر تبدیل می کند.";
                            break;
                        case Data.BuildingID.airdefense:
                            _descriptionText.text = "این یک برجک قدرتمند است که منحصراً دشمنان هوایی را با آسیب بسیار زیاد هدف قرار می دهد. دسترسی خوب و امتیازات مناسبی دارد. این ساختمان ها فقط می توانند یک نیروی هوایی را در یک زمان هدف قرار دهند و به آنها شلیک کنند.";
                            break;
                        case Data.BuildingID.wizardtower:
                            _descriptionText.text = "این ساختمان می‌تواند آسیب‌های پاشش شدیدی به واحدهای زمینی و هوایی وارد کند، اگرچه به برد نسبتاً کوتاهی محدود است.";
                            break;
                        case Data.BuildingID.infernotower:
                            _descriptionText.text = "این یکی از ترسناک ترین دفاع ها در بازی است. این ساختمان جریانی از شعله پرتاب می کند که حتی ضخیم ترین زره ها را می سوزاند.";
                            break;
                        case Data.BuildingID.clancastle:
                            _descriptionText.text = "این ساختمان برای ایجاد یا پیوستن به یک قبیله مورد نیاز است. با حضور در یک قبیله می توانید در جنگ ها شرکت کنید و در طول بازی با هم قبیله های خود پیشرفت کنید.";
                            break;
                        case Data.BuildingID.spellfactory:
                            _descriptionText.text = "این ساختمان به بازیکن اجازه می دهد تا طلسم ایجاد کند. شما می توانید از توانایی های جادوها برای به دست آوردن مزیت در نبرد استفاده کنید. استفاده از جادوها در زمان مناسب و روی هدف مناسب می تواند باعث تغییر سرنوشت نبرد شود.";
                            break;
                        case Data.BuildingID.laboratory:
                            _descriptionText.text = "این ساختمانی است که در آن می‌توانید سربازان و طلسم‌های خود را ارتقا دهید و باعث افزایش خسارت، قدرت دفاعی و سایر خصوصیات آنها شوید.";
                            break;
                        case Data.BuildingID.obstacle:
                            _descriptionText.text = "طبیعت اطراف بازی که در صورت عدم رسیدگی به دهکده خود افزایش پیدا می کند.";
                            /*
                            switch (level)
                            {
                                case 1: _descriptionText.text = ""; break;
                                case 2: _descriptionText.text = ""; break;
                                case 3: _descriptionText.text = ""; break;
                                case 4: _descriptionText.text = ""; break;
                                case 5: _descriptionText.text = ""; break;
                            }
                            */
                            break;
                        default:
                            _descriptionText.text = "";
                            break;
                    }
                    break;
                default:
                    _descriptionText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    switch (id)
                    {
                        case Data.BuildingID.townhall:
                            _descriptionText.text = "This is the main building of your settelment. Upgrading it will unlock new buildings, troops and spells. Protection of this building is critical and enemy forces will achive one star just for destroying it in battle.";
                            break;
                        case Data.BuildingID.goldmine:
                            _descriptionText.text = "This building collects Gold from an unlimited underground reserve and stores it until collected by the player and placed into a storage. When the building is full, production will be stopped until it is collected or raided by an enemy player.";
                            break;
                        case Data.BuildingID.goldstorage:
                            _descriptionText.text = "All your precious Gold is stored here. This building allows players to save hard earned Gold so that it can be used for future upgrades. Protection of this building is critical and enemy forces can steal the Gold inside it during battle.";
                            break;
                        case Data.BuildingID.elixirmine:
                            _descriptionText.text = "This building collects Elixir from an unlimited underground reserve and stores it until collected by the player and placed into a storage. When the building is full, production will be stopped until it is collected or raided by an enemy player.";
                            break;
                        case Data.BuildingID.elixirstorage:
                            _descriptionText.text = "All your precious Elixir is stored here. This building allows players to save hard earned Elixir so that it can be used for future upgrades. Protection of this building is critical and enemy forces can steal the Elixir inside it during battle.";
                            break;
                        case Data.BuildingID.buildershut:
                            _descriptionText.text = "Nothing gets done around here without Builders. Each of this building can only hold one Builder at a time. You can not build or upgrade anything unless you have free Builders.";
                            break;
                        case Data.BuildingID.armycamp:
                            _descriptionText.text = "Your troops are stationed here. This is the building that unlocks troop capacity. Higher troop capacity allows you to have a larger army.";
                            break;
                        case Data.BuildingID.barracks:
                            _descriptionText.text = "This building allow you to train troops to attack your enemies. Upgrade it to unlock advanced units that can win epic battles. Each specific level unlocks a new troop. Queued troops can be cancelled at any time but the resources will not be refunded.";
                            break;
                        case Data.BuildingID.wall:
                            _descriptionText.text = "The main purpose of Walls is to hinder attacking ground troops, allowing defenses to damage and kill attackers as they attempt to breach the Walls. Once breaches occur, the attacking troops have free rein to destroy the defenses and other buildings within the Walls.";
                            break;
                        case Data.BuildingID.cannon:
                            _descriptionText.text = "This is a single-target defense that deals moderate damage. It is the first defensive structure that a player builds at the start of their adventure. This building is both cheap and quick to upgrade at lower levels.";
                            break;
                        case Data.BuildingID.archertower:
                            _descriptionText.text = "It is a single-target defense in the game. They are the second defense available to players and the first defense to attack aerial troops. These buildings are extremely versatile structures. They are able to target both ground and air units, and they have excellent range.";
                            break;
                        case Data.BuildingID.mortor:
                            _descriptionText.text = "They shoot long-range explosive shells which deal devastating splash damage to every ground unit within a small radius of the impact point. Their splash damage, combined with their long-range, makes them deadly weapons against large groups of weaker enemies.";
                            break;
                        case Data.BuildingID.airdefense:
                            _descriptionText.text = "It is a powerful turret that exclusively targets aerial foes with very high damage. It has good reach and decent hitpoints. These buildings can only target and shoot one air troop at a time.";
                            break;
                        case Data.BuildingID.wizardtower:
                            _descriptionText.text = "This building is capable of inflicting powerful splash damage to both ground and air units, though it is limited to a relatively short range.";
                            break;
                        case Data.BuildingID.infernotower:
                            _descriptionText.text = "It is one of the most feared defenses on the game. This building shoots a stream of flame that burns through even the thickest armor.";
                            break;
                        case Data.BuildingID.clancastle:
                            _descriptionText.text = "This building is needed for creating or joining a Clan. By being in a Clan you can participate in wars and progress through the game with your clanmates.";
                            break;
                        case Data.BuildingID.spellfactory:
                            _descriptionText.text = "This building allows the player to create spells. You can use the spells abilities to gain advantage in battle. Useing the spells at the right time and on the right target cange change the corse of battle.";
                            break;
                        case Data.BuildingID.laboratory:
                            _descriptionText.text = "This is a building where you can upgrade your Troops and Spells by improving their stats such as damage and hitpoints.";
                            break;
                        case Data.BuildingID.obstacle:
                            switch (level)
                            {
                                case 1: _descriptionText.text = ""; break;
                                case 2: _descriptionText.text = ""; break;
                                case 3: _descriptionText.text = ""; break;
                                case 4: _descriptionText.text = ""; break;
                                case 5: _descriptionText.text = ""; break;
                            }
                            break;
                        default:
                            _descriptionText.text = "";
                            break;
                    }
                    break;
            }
            _active = true;
            transform.SetAsLastSibling();
            _elements.SetActive(true);
            _titleText.ForceMeshUpdate(true);
            _descriptionText.ForceMeshUpdate(true);
        }

        public void OpenSpellInfo(Data.SpellID id)
        {
            Sprite icon = AssetsBank.GetSpellIcon(id);
            if (icon != null)
            {
                _icon.sprite = icon;
            }
            _titleText.text = Language.instanse.GetSpellName(id);
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    _descriptionText.horizontalAlignment = HorizontalAlignmentOptions.Right;
                    switch (id)
                    {
                        case Data.SpellID.lightning:
                            _descriptionText.text = "این طلسم در شعاع کمی به ساختمان ها و نیروهای دشمن آسیب می زند.";
                            break;
                        case Data.SpellID.healing:
                            _descriptionText.text = "این طلسم حلقه ای در میدان نبرد ایجاد می کند که تمام نیروهای داخل آن را درمان میکند.";
                            break;
                        case Data.SpellID.rage:
                            _descriptionText.text = "این طلسم حلقه‌ای را در میدان جنگ ایجاد می‌کند که سرعت حرکت و خسارت هر سرباز خودی را در داخل آن افزایش می‌دهد.";
                            break;
                        case Data.SpellID.freeze:
                            _descriptionText.text = "این طلسم برای غیرفعال کردن موقت سیستم های دفاعی دشمن در شعاع کوچک استفاده می شود.";
                            break;
                        case Data.SpellID.invisibility:
                            _descriptionText.text = "این طلسم باعث می شود که سیستم دفاعی دشمن نیروهای شما را که در شعاع طلسم هستند نبینند. گلوله هایی که قبل از نامرئی شدن شلیک میشوند همچنان به هدف خود ضربه میزنند.";
                            break;
                        default:
                            _descriptionText.text = "";
                            break;
                    }
                    break;
                default:
                    _descriptionText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                    switch (id)
                    {
                        case Data.SpellID.lightning:
                            _descriptionText.text = "This spell damages enemy buildings and troops in a small radius.";
                            break;
                        case Data.SpellID.healing:
                            _descriptionText.text = "This spell creates a ring on the battlefield that heals all troops inside.";
                            break;
                        case Data.SpellID.rage:
                            _descriptionText.text = "This spell creates a ring on the battlefield that boosts the movement speed and damage of any friendly troops inside it.";
                            break;
                        case Data.SpellID.freeze:
                            _descriptionText.text = "This spell is used to temporarily disable enemy defenses within a small radius.";
                            break;
                        case Data.SpellID.invisibility:
                            _descriptionText.text = "This spell used to temporarily make the defenses can't see your troops who are in this spell radius. Defenses that are fired before they went invisible will still hit their target.";
                            break;
                        default:
                            _descriptionText.text = "";
                            break;
                    }
                    break;
            }
            _active = true;
            transform.SetAsLastSibling();
            _elements.SetActive(true);
            _titleText.ForceMeshUpdate(true);
            _descriptionText.ForceMeshUpdate(true);
        }

    }
}