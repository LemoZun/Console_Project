namespace console
{
    internal class Program
    {
        struct Character
        {
            public int number;
            public Name name;
            public Classification job;
            public int hp;
            public float mp;
            // private float MIN_MP = 0;
            public Skill[] skill;

        }
        struct Result
        {
            public int damage;
            public Judgement judgement;
            public int doEx;
        }
        enum Name { 솔, 고우키, 폴, 가일 }
        enum Classification { Balance, OneShot, Ranged }
        enum Skill { 가드, 잡기, 점프공격, 승룡권, 장풍, 필살기, 없음 }
        enum Judgement { 피격, 타격, 상쇄 }

        static void Main(string[] args)
        {
            int count = 1;
            Character player = new Character();
            Character enemy = new Character();

            Character[] characterEntry = new Character[4];
            // int count = 0;
            Character solBadguy = new Character();
            solBadguy = CharacterSet(solBadguy, 1, Name.솔, Classification.Balance, 100, 0);

            Character gouki = new Character();
            gouki = CharacterSet(gouki, 2, Name.고우키, Classification.Balance, 100, 0);

            Character paulPheonix = new Character();
            paulPheonix = CharacterSet(paulPheonix, 3, Name.폴, Classification.OneShot, 100, 50);

            Character guile = new Character();
            guile = CharacterSet(paulPheonix, 4, Name.가일, Classification.Ranged, 100, 0);

            characterEntry[0] = solBadguy;
            characterEntry[1] = gouki;
            characterEntry[2] = paulPheonix;
            characterEntry[3] = guile;

            while (true)
            {
                Console.WriteLine("");
                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("=                                  =");
                Console.WriteLine("=   스트리트 철권 : 스트라이브     =");
                Console.WriteLine("=                                  =");
                Console.WriteLine("====================================");
                Console.WriteLine();
                Console.WriteLine("    계속하려면 아무키나 누르세요    ");
                Console.ReadKey();
                player = CharacterSelect(characterEntry);
                Random rand = new Random();
                enemy = characterEntry[rand.Next(0, 4)];
                // Console.WriteLine($"{player.name}를 선택했습니다.");
                Console.WriteLine($"상대 캐릭터는 {enemy.name} 입니다.");
                Thread.Sleep(1500);
                Console.Clear();

                while (true)
                {
                    int playerSkNum;
                    int enemySkNum;
                    Result fight;
                    int check;
                    Console.WriteLine("========================================================");
                    Console.WriteLine("=                                                      =");
                    if (count < 10)
                    {
                        Console.WriteLine($"={count,+28}합                        =");
                    }
                    else
                    {
                        Console.WriteLine($"={count,+29}합                       =");
                    }
                    Console.WriteLine("=                                                      =");
                    Console.WriteLine("========================================================");
                    Console.WriteLine();

                    Console.Write($"플레이어 : {player.name,-34}상대:{enemy.name}");                    
                    Console.WriteLine();

                    // Console.WriteLine($"####################                ####################"); //' '17칸
                    int hpBarforPlayer = player.hp / 5;
                    int hpBarforEnemy = enemy.hp / 5;
                    int mpBarforPlayer = (int)player.mp / 5;

                    int mpBarforEnemy = (int)enemy.mp / 5;

                    Console.ForegroundColor = ConsoleColor.Red;
                    BarPrint(hpBarforPlayer, hpBarforEnemy); //hp 출력
                    Console.ResetColor();
                    Console.WriteLine("\n");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    BarPrint(mpBarforPlayer, mpBarforEnemy); //mp 출력
                    Console.ResetColor();

                    Console.WriteLine("\n");
                    Console.WriteLine($"플레이어 HP : {player.hp,-28}상대 HP : {enemy.hp}");
                    Console.WriteLine($"플레이어 MP : {player.mp,-28}상대 MP : {enemy.mp}");

                    Console.WriteLine("\n");
                    playerSkNum = PlayerSkillSellect(ref player);
                    if (playerSkNum == -1)
                    {
                        Thread.Sleep(1500);
                        Console.Clear();
                        continue;
                    }
                    enemySkNum = EnemySkillSellct(enemy);

                    fight = HitBox(player.skill[playerSkNum - 1], enemy.skill[enemySkNum], player, enemy); //플레이어의 선택지는 1~6이고 스킬배열은 0~5니까 -1 해야함
                    DamageCal(fight, ref player, ref enemy);
                    count++;

                    Thread.Sleep(2000);
                    Console.Clear();
                    
                    check = AreYouKO(player, enemy);
                    if (check == 1)
                    {
                        break;
                    }
                    else if(check == 0)
                    {                        
                        Environment.Exit(0);
                    }                    
                }
            }
        }
        /// <summary>
        /// 캐릭터를 만드는 함수
        /// </summary>
        /// <param name="c">캐릭터 구조체</param>
        /// <param name="count">몇번째 캐릭터인지 숫자</param>
        /// <param name="name">캐릭터의 이름</param>
        /// <param name="job">캐릭터의 성향</param>
        /// <param name="hp">캐릭터의 체력</param>
        /// <param name="mp">캐릭터의 마나</param>
        /// <returns></returns>
        static Character CharacterSet(Character c, int count, Name name, Classification job, int hp, int mp)
        {
            // count ++;
            c.number = count;
            c.name = name;
            c.job = job;
            c.hp = hp;
            c.mp = mp;
            c.skill = new Skill[6];
            c.skill[0] = Skill.가드;
            c.skill[1] = Skill.잡기;
            c.skill[2] = Skill.점프공격;
            c.skill[3] = Skill.필살기;

            if (job == Classification.Balance)
            {
                c.skill[4] = Skill.승룡권;
                c.skill[5] = Skill.장풍;
            }
            if (job == Classification.OneShot)
            {
                c.skill[4] = Skill.승룡권;
                c.skill[5] = Skill.없음;
            }
            if (job == Classification.Ranged)
            {
                c.skill[4] = Skill.없음;
                c.skill[5] = Skill.장풍;
            }

            return c;

        } /**캐릭터 생성 */    
        
        static Character CharacterSelect(Character[] entry) 
        {
            int characterNum;
            bool toDetermine;
            while (true)
            {

                Console.Clear();
                Console.WriteLine("====================================");
                Console.WriteLine("=  1. 솔 배드가이     3.폴 피닉스  =");
                Console.WriteLine("=                                  =");
                Console.WriteLine("=  2. 고우키          4.가일       =");
                Console.WriteLine("====================================");
                Console.WriteLine();
                Console.WriteLine("       캐릭터 번호를 선택하세요       ");

                toDetermine = int.TryParse(Console.ReadLine(), out characterNum);
                if (toDetermine == false)
                {
                    //Console.Clear();
                    Console.WriteLine("선택할 캐릭터 '번호'를 입력 해 주세요.");
                    Thread.Sleep(1500);
                    continue;
                }
                else if (characterNum < 1 || characterNum > 4)
                {
                    //Console.Clear();
                    Console.WriteLine("선택한 번호에 캐릭터가 없습니다.");
                    Console.WriteLine("캐릭터가 있는 번호를 입력해주세요");
                    Thread.Sleep(1500);
                    continue;
                }
                else
                {
                    Console.WriteLine($"{entry[characterNum - 1].name}을 선택하셨습니다.");
                    return entry[characterNum - 1];
                }
            }

        }

        /// <summary>
        /// 공격 판정을 보조하는 함수
        /// </summary>
        /// <param name="j">공격 판정이 타격,피격,상쇄인지 받는다</param>
        /// <param name="damage">데미지가 양수면 플레이어가 가한 데미지, 음수면 플레이어가 피격당한 데미지</param>
        /// <param name="doEx">필살기를 썼는지, 썼다면 누가 썼는지 받는다</param>
        /// <returns>결과를 Result 구조체로 반환한다</returns>
        static Result Judge(Judgement j, int damage, int doEx) // 판정 보조
        {
            Result result;
            result.judgement = j;
            result.damage = damage;
            result.doEx = doEx;
            return result;
        }

        /// <summary>
        /// 필살기를 누가 썼는지 결정하는 함수
        /// </summary>
        /// <param name="playerSkill">플레이어의 스킬을 받아 필살기인지 봄</param>
        /// <param name="enemySkill">적의 스킬을 받아 필살기인지 봄</param>
        /// <returns>0은 둘 다 사용하지 않음, 1은 플레이어가 필살기를 사용, 2는 적이 필살기를 사용, 3은 둘 다 필살기를 사용한 경우</returns>
        static int DoEX(Skill playerSkill, Skill enemySkill) 
        {
            int i;
            if (playerSkill == Skill.필살기 && enemySkill != Skill.필살기)
            {
                i = 1;
                return i;
            }
            else if (playerSkill != Skill.필살기 && enemySkill == Skill.필살기)
            {
                i = 2;
                return i;
            }
            else if (playerSkill == Skill.필살기 && enemySkill == Skill.필살기)
            {
                i = 3;
                return i;
            }
            else
            {
                i = 0;
                return i;
            }
        }

        /// <summary>
        /// 전체적인 스킬 판정 결과와 데미지 결과를 결정하는 함수
        /// </summary>
        /// <param name="playerSkill">플레이어가 무슨 스킬을 썼는지 받는다</param>
        /// <param name="enemySkill">적이 무슨 스킬을 썼는지 받는다</param>
        /// <param name="player">플레이어의 캐릭터 성향 분류에따라 데미지 보정을 받기위해 플레이어 캐릭터를 입력받는다</param>
        /// <param name="enemy">적의 캐릭터 성향 분류에따라 데미지 보정을 받기위해 플레이어 캐릭터를 입력받는다</param>
        /// <returns>스킬판정 결과와 데미지, 필살기 사용 유무를 판단하는 Result 구조체를 리턴</returns>
        static Result HitBox(Skill playerSkill, Skill enemySkill, Character player, Character enemy)
        {
            /* 가드 : 대부분의 공격을 상쇄+가드데미지를 입음, 필살기 막으면 확정반격, 잡기에 짐
             * 잡기 : 가드와 승룡을 이김, 필살기와 점프공격, 장풍에 짐 나머지 상쇄
             * 점프공격 : 잡기와 장풍에 이김, 승룡과 필살기에 짐, 나머지 상쇄
             * 승룡 : 점공에 이김, 잡기에짐, 나머지 상쇄 // 필살기에 상쇄나게 만들까
             * 장풍 : 잡기에 이김, 점프공격과 필살기에 짐, 나머지 상쇄
             * 필살기 : 대부분의 공격에 이김, 가드에 짐, 승룡에 상쇄
             * 없음 : 관망상태, 레버 놓기
             * 
            */
            Result result;

            switch (playerSkill)
            {
                case Skill.가드: // 내 스킬

                    switch (enemySkill) // 적 스킬
                    {
                        case Skill.가드:
                            {
                                //result.judgement = Judgement.무승부;
                                //result.damage = 0;
                                //return result;
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        case Skill.잡기:
                            {
                                result = Judge(Judgement.피격, -20, 0);
                                return result;
                            }
                        case Skill.필살기:
                            {
                                result = Judge(Judgement.타격, 15, 2);// 확정반격
                                Console.WriteLine("필살기를 막고 확정반격을 했습니다.");
                                return result;
                            }
                        case Skill.승룡권:
                            {
                                result = Judge(Judgement.타격, 15, 0);// 확정반격
                                Console.WriteLine("승룡을 막고 확정반격을 했습니다.");
                                return result;
                            }
                        case Skill.없음:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        case Skill.장풍:
                            {
                                if (enemy.job == Classification.Ranged)
                                {
                                    result = Judge(Judgement.상쇄, -15, 0);
                                    Console.WriteLine("가드 데미지를 받았습니다.");
                                    return result;
                                }
                                else
                                {
                                    result = Judge(Judgement.상쇄, -5, 0);
                                    Console.WriteLine("가드 데미지를 받았습니다.");
                                    return result;
                                }
                            }
                        default:
                            {
                                result = Judge(Judgement.상쇄, -5, 0); //가드 데미지
                                Console.WriteLine("가드 데미지를 받았습니다.");
                                return result;
                            }
                    }
                case Skill.잡기:

                    switch (enemySkill)
                    {
                        case Skill.가드:
                            {
                                result = Judge(Judgement.타격, 20, 0);
                                return result;
                            }
                        case Skill.잡기:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        case Skill.점프공격:
                            {
                                result = Judge(Judgement.피격, -10, 0);
                                return result;
                            }
                        case Skill.승룡권:
                            {
                                result = Judge(Judgement.타격, 20, 0);
                                return result;
                            }
                        case Skill.장풍:
                            {
                                if (enemy.job == Classification.Ranged)
                                {
                                    result = Judge(Judgement.피격, -20, 0);
                                    return result;
                                }
                                else
                                {
                                    result = Judge(Judgement.피격, -10, 0);
                                    return result;
                                }
                            }
                        case Skill.필살기:
                            {
                                result = Judge(Judgement.피격, -50, 2);
                                return result;
                            }
                        case Skill.없음:
                            {
                                result = Judge(Judgement.타격, 20, 0);
                                return result;
                            }
                        default:
                            {
                                result = Judge(Judgement.타격, 20, 0);
                                return result;
                            }
                    }

                case Skill.점프공격:

                    switch (enemySkill)
                    {
                        case Skill.가드:
                            {
                                result = Judge(Judgement.상쇄, +5, 0);
                                Console.WriteLine("적이 가드 데미지를 받았습니다.");
                                return result;
                            }
                        case Skill.잡기:
                            {
                                result = Judge(Judgement.타격, 10, 0);
                                return result;
                            }
                        case Skill.점프공격:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        case Skill.승룡권:
                            {
                                result = Judge(Judgement.피격, -10, 0);
                                return result;
                            }
                        case Skill.장풍:
                            {
                                result = Judge(Judgement.타격, 10, 0);
                                return result;
                            }
                        case Skill.필살기:
                            {
                                result = Judge(Judgement.피격, -50, 2);
                                return result;
                            }
                        case Skill.없음:
                            {
                                result = Judge(Judgement.타격, 10, 0);
                                return result;
                            }
                        default:
                            {
                                result = Judge(Judgement.타격, 10, 0);
                                return result;
                            }
                    }

                case Skill.승룡권:

                    switch (enemySkill)
                    {
                        case Skill.가드:
                            {
                                result = Judge(Judgement.피격, -15, 0);// 확정반격
                                Console.WriteLine("승룡을 막혀 확정반격 당했습니다.");
                                return result;
                            }
                        case Skill.잡기:
                            {
                                result = Judge(Judgement.피격, -20, 0);
                                return result;
                            }
                        case Skill.점프공격:
                            {
                                result = Judge(Judgement.타격, 10, 0);
                                return result;
                            }
                        case Skill.승룡권:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        case Skill.장풍:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        case Skill.필살기:
                            {
                                result = Judge(Judgement.상쇄, 0, 2);
                                return result;
                            }
                        case Skill.없음:
                            {
                                result = Judge(Judgement.타격, 10, 0);
                                return result;
                            }
                        default:
                            {
                                result = Judge(Judgement.타격, 10, 0);
                                return result;
                            }
                    }
                case Skill.장풍:

                    switch (enemySkill)
                    {
                        case Skill.가드:
                            {
                                if (player.job == Classification.Ranged)
                                {
                                    result = Judge(Judgement.상쇄, 15, 0);
                                    Console.WriteLine("적이 가드 데미지를 받았습니다.");
                                    return result;
                                }
                                else
                                {
                                    result = Judge(Judgement.상쇄, 5, 0);
                                    Console.WriteLine("적이 가드 데미지를 받았습니다.");
                                    return result;
                                }
                            }
                        case Skill.잡기:
                            {
                                if (player.job == Classification.Ranged)
                                {
                                    result = Judge(Judgement.타격, 20, 0);
                                    return result;
                                }
                                else
                                {
                                    result = Judge(Judgement.타격, 10, 0);
                                    return result;
                                }
                            }
                        case Skill.점프공격:
                            {
                                result = Judge(Judgement.피격, -10, 0);
                                return result;
                            }
                        case Skill.승룡권:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        case Skill.장풍:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        case Skill.필살기:
                            {
                                result = Judge(Judgement.피격, -50, 2);
                                return result;
                            }
                        case Skill.없음:
                            {
                                if (player.job == Classification.Ranged)
                                {
                                    result = Judge(Judgement.타격, 20, 0);
                                    return result;
                                }
                                else
                                {
                                    result = Judge(Judgement.타격, 10, 0);
                                    return result;
                                }
                            }
                        default:
                            {
                                if (player.job == Classification.Ranged)
                                {
                                    result = Judge(Judgement.타격, 20, 0);
                                    return result;
                                }
                                else
                                {
                                    result = Judge(Judgement.타격, 10, 0);
                                    return result;
                                }
                            }
                    }
                case Skill.필살기:

                    switch (enemySkill)
                    {
                        case Skill.가드:
                            {
                                result = Judge(Judgement.피격, -15, 1);
                                Console.WriteLine("적이 필살기를 막고 확정반격을 했습니다.");
                                return result;
                            }
                        case Skill.잡기:
                            {
                                result = Judge(Judgement.타격, 50, 1);
                                return result;
                            }
                        case Skill.점프공격:
                            {
                                result = Judge(Judgement.타격, 50, 1);
                                return result;
                            }
                        case Skill.승룡권:
                            {
                                result = Judge(Judgement.상쇄, 0, 1);
                                return result;
                            }
                        case Skill.장풍:
                            {
                                result = Judge(Judgement.타격, 50, 1);
                                return result;
                            }
                        case Skill.필살기:
                            {
                                result = Judge(Judgement.상쇄, 0, 3);
                                return result;
                            }
                        case Skill.없음:
                            {
                                result = Judge(Judgement.타격, 50, 1);
                                return result;
                            }
                        default:
                            {
                                result = Judge(Judgement.타격, 50, 1);
                                return result;
                            }
                    }
                case Skill.없음:

                    switch (enemySkill)
                    {
                        case Skill.가드:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        case Skill.잡기:
                            {
                                result = Judge(Judgement.피격, -20, 0);
                                return result;
                            }
                        case Skill.점프공격:
                            {
                                result = Judge(Judgement.피격, -10, 0);
                                return result;
                            }
                        case Skill.승룡권:
                            {
                                result = Judge(Judgement.피격, -10, 0);
                                return result;
                            }
                        case Skill.장풍:
                            {
                                if (enemy.job == Classification.Ranged)
                                {
                                    result = Judge(Judgement.피격, -20, 0);
                                    return result;
                                }
                                else
                                {
                                    result = Judge(Judgement.피격, -10, 0);
                                    return result;
                                }
                            }
                        case Skill.필살기:
                            {
                                result = Judge(Judgement.피격, -50, 2);
                                return result;
                            }
                        case Skill.없음:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                        default:
                            {
                                result = Judge(Judgement.상쇄, 0, 0);
                                return result;
                            }
                    }
                default:
                    {
                        result = Judge(Judgement.상쇄, 0, 0);
                        return result;
                    }

            }
        }

        /// <summary>
        /// 플레이어가 무슨 스킬을 쓸지 결정하는 함수
        /// </summary>
        /// <param name="player">플레이어 캐릭터를 입력받음</param>
        /// <returns>무슨 스킬을 사용했는지 스킬넘버를 리턴</returns>
        static int PlayerSkillSellect(ref Character player)
        {
            Console.WriteLine("사용할 기술을 선택 해 주세요.");
            Console.WriteLine($"1.{player.skill[0]}, 2.{player.skill[1]}, 3.{player.skill[2]}, 4.{player.skill[3]}, 5.{player.skill[4]}, 6.{player.skill[5]}");
            int skillNum;
            bool toDetermine;
            while (true)
            {
                toDetermine = int.TryParse(Console.ReadLine(), out skillNum);
                if (toDetermine == false)
                {
                    Console.WriteLine("사용할 스킬 '번호'를 입력해 주세요.");
                    skillNum = -1;
                    return skillNum;
                }
                else if (skillNum < 1 || skillNum > 6)
                {
                    Console.WriteLine("1~6 안의 스킬 숫자를 입력해 주세요.");
                    skillNum = -1;
                    return skillNum;
                }
                else if (skillNum == 4 && player.mp != 100)
                {
                    Console.WriteLine("필살기를 쓰기 위한 마나가 부족합니다.");
                    Console.WriteLine("필살기는 마나가 100일때 쓸 수 있습니다.");
                    Console.WriteLine("사용할 스킬을 다시 선택 해 주세요.");
                    skillNum = -1;
                    return skillNum;
                    // continue;

                }
                else if (skillNum == 4 && player.mp == 100)
                {
                    player.mp = 0;
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"{player.skill[skillNum - 1]}");
                    Console.ResetColor();
                    Console.WriteLine("을(를) 사용했습니다.");
                    return skillNum;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{player.skill[skillNum - 1]}");
                    Console.ResetColor();
                    Console.WriteLine("을(를) 사용했습니다.");
                    return skillNum;
                }
            }
        }

        /// <summary>
        /// 적이 무슨 스킬을 쓸지 결정하는 함수
        /// </summary>
        /// <param name="enemy">적으로 선택된 캐릭터를 받는다</param>
        /// <returns>적이 사용한 스킬넘버를 반환</returns>
        static int EnemySkillSellct(Character enemy)
        {
            int skillNum;
            Random rand = new Random();
            skillNum = rand.Next(0, 6);
            while (true)
            {
                if (enemy.skill[skillNum] == Skill.없음)
                {
                    skillNum = rand.Next(0, 6);
                }
                else if (enemy.skill[skillNum] == Skill.필살기 && enemy.mp != 100)
                {
                    // Console.WriteLine("적 필살기 사용, 마나 100 안되서 다시 선택");
                    skillNum = rand.Next(0, 6);
                    continue; // 여기선 붙이나 안붙이나 관계 없나
                }
                else
                {
                    break;
                }
            }
            Console.Write("적이");
            if (enemy.skill[skillNum] == Skill.필살기)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.Write($"{enemy.skill[skillNum]}");
            Console.ResetColor();
            Console.WriteLine("을(를) 사용했습니다.");

            return skillNum;

        }

        /// <summary>
        /// 타격이나 피격시 차오르는 플레이어와 적의 마나를 결정하기 위해 사용함
        /// 
        /// </summary>
        /// <param name="result">데미지와 공격판정 결과, 누가 필살기를 썼는지 받는다</param>
        /// <param name="player">플레이어의 캐릭터</param>
        /// <param name="enemy">적의 캐릭터</param>
        static void AddMP(Result result, ref Character player, ref Character enemy)
        {
            //피격시
            if (result.doEx == 0 && result.damage < 0) // 둘 다 필살기 미사용, 플레이어 데미지 받았을 때
            {
                player.mp += 1 * Math.Abs(result.damage);
                enemy.mp += 2 * Math.Abs(result.damage);
            }
            else if (result.doEx == 1 && result.damage <= 0) //플레이어가 필살기를 썼고 데미지를 받았을 때 = 플레이어 필살기, 상대 가드
            {
                player.mp = 0;
                enemy.mp += 2 * Math.Abs(result.damage);
            }
            else if (result.doEx == 2 && result.damage <= 0) // 적이 필살기를 썼고  플레이어가 데미지를 받았을 때
            {
                player.mp += 1 * Math.Abs(result.damage);
                enemy.mp = 0;
            }
            else if (result.doEx == 3) // 둘 다 필살기를 썼을때
            {
                player.mp = 0;
                enemy.mp = 0;
            }
            else if (result.doEx == 0 && result.damage > 0) // 둘 다 필살기 미사용, 플레이어가 데미지를 줄 때
            {
                player.mp += 2 * Math.Abs(result.damage);
                enemy.mp += 1 * Math.Abs(result.damage);
            }
            else if (result.doEx == 1 && result.damage >= 0) //플레이어 필살기 사용, 플레이어가 데미지를 줄 때
            {
                player.mp = 0;
                enemy.mp += 1 * Math.Abs(result.damage);
            }
            else if (result.doEx == 2 && result.damage >= 0) // 적 필살기 사용, 플레이어가 데미지를 줌 = 플레이어 가드, 상대 필살기
            {
                player.mp += 1 * Math.Abs(result.damage);
                enemy.mp = 0;
            }

            if (player.mp > 100)
            {
                player.mp = 100;
            }
            if (enemy.mp > 100)
            {
                enemy.mp = 100;
            }
            if (player.mp < 0)
            {
                player.mp = 0;
            }
            if (enemy.mp < 0)
            {
                enemy.mp = 0;
            }

            if (result.doEx < 0 || result.doEx > 3) // doex에 이상한 값이 혹시나 들어갔을때
            {
                Console.WriteLine("필살기 분석 오류");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 데미지를 받아 캐릭터의 hp에 변화를 주는 함수
        /// </summary>
        /// <param name="result">데미지와 판정결과를 받는다. 필살기를 받는 이유는 고우키가 막타를 필살기로 때렸을 때 특수창을 띄우기 위함</param>
        /// <param name="player">플레이어의 이름과 hp를 받기위해 캐릭터를 받음</param>
        /// <param name="enemy">적의 이름과 hp를 받기위해 캐릭터를 받음</param>
        static void DamageCal(Result result, ref Character player, ref Character enemy)
        {
            if (result.damage < 0)
            {
                player.hp += result.damage;
                Console.Write($"{-result.damage}의");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" 데미지를 받았습니다.");
                Console.ResetColor();

                if (player.hp < 0)
                {
                    player.hp = 0;
                }
                else if (enemy.hp < 0)
                {
                    enemy.hp = 0;
                }

                Console.WriteLine($"플레이어의 HP : {player.hp} 적의 HP : {enemy.hp}");


                // return;
            }
            else if (result.damage > 0)
            {
                enemy.hp -= result.damage;
                Console.Write($"{result.damage}의");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("데미지를 주었습니다.");
                Console.ResetColor();
                if (player.hp < 0)
                {
                    player.hp = 0;
                }
                else if (enemy.hp < 0)
                {
                    enemy.hp = 0;
                }
                Console.WriteLine($"플레이어의 HP : {player.hp} 적의 HP : {enemy.hp}");

                //if ((player.name == Name.고우키 || enemy.name == Name.고우키) && result.doEx != 0 && (enemy.hp <= 0 || player.hp <= 0)) //고우키로 막타를 필살기로 했을 경우 // 가독성이 좋지 않은 조건은 빈틈이 나온다..
                if(player.name == Name.고우키 && result.doEx == 1 && enemy.hp<=0)
                {
                    Thread.Sleep(1500);
                    Console.Clear();
                    Console.WriteLine("========================================================");
                    Console.WriteLine("=                                                      =");
                    Console.WriteLine($"=                          天                          =");
                    Console.WriteLine("=                                                      =");
                    Console.WriteLine("========================================================");
                    Thread.Sleep(1500);

                    Environment.Exit(0);
                }
                else if(enemy.name == Name.고우키 && result.doEx==2 && player.hp<=0)
                {
                    Thread.Sleep(1500);
                    Console.Clear();
                    Console.WriteLine("========================================================");
                    Console.WriteLine("=                                                      =");
                    Console.WriteLine($"=                          天                          =");
                    Console.WriteLine("=                                                      =");
                    Console.WriteLine("========================================================");
                    Thread.Sleep(1500);

                    Environment.Exit(0);
                }
                // return;
            }
            else
            {
                Console.WriteLine("서로의 공격을 상쇄했습니다.");
                Console.WriteLine($"플레이어의 HP : {player.hp} 적의 HP : {enemy.hp}");
                // return;
            }
            AddMP(result, ref player, ref enemy);
            return;
        }

        /// <summary>
        /// hp가 0일때 승패판정을 하기 위한 함수
        /// </summary>
        /// <param name="player">플레이어의 hp를 받기 위해 받음</param>
        /// <param name="enemy">적의 hp를 받기 위해 받음</param>
        /// <returns>게임을 다시 할건지 결정하는 코인 변수를 받음 1이 다시하기 그 이외에는 게임 종료</returns>         
        static int AreYouKO(Character player, Character enemy)
        {
            int coin = -1;
            bool check;
            if (player.hp <= 0)
            {
                Console.WriteLine("========================================================");
                Console.WriteLine("=                                                      =");
                Console.WriteLine($"=                  당신의 패배입니다!                  =");
                Console.WriteLine("=                                                      =");
                Console.WriteLine("========================================================");

                check = BeGoingtoContinue();

                if (check == true)
                {
                    coin = 1;
                    return coin;
                }
                else if (check == false)
                {
                    coin = 0;
                    return coin;
                }
                else return coin;

            }
            else if (enemy.hp <= 0)
            {
                Console.WriteLine("========================================================");
                Console.WriteLine("=                                                      =");
                Console.WriteLine($"=                 당신의 승리입니다!                   =");
                Console.WriteLine("=                                                      =");
                Console.WriteLine("========================================================");

                check= BeGoingtoContinue();

                if (check == true)
                {
                    coin = 1;
                    return coin;
                }
                else if (check == false)
                {
                    coin = 0;
                    return coin;
                }
                else return coin;
                //Environment.Exit(0);
            }
            else return coin;

        }

        /// <summary>
        /// 게임을 다시 할거냐 묻는 함수
        /// </summary>
        /// <returns>계속할거냐의 Y를 받으면 코인 변수를 true로 그 이외에는 false로 리턴</returns>
        static bool BeGoingtoContinue()
        {
            Console.WriteLine("Continue?");
            Console.WriteLine("Y/N");
            ConsoleKeyInfo toBeContinue = Console.ReadKey(false);
            bool coin = false;

            if (toBeContinue.Key == ConsoleKey.Y)
            {
                Console.WriteLine("\n");
                Console.WriteLine("Here Comes a New Challenger");
                Thread.Sleep(2000);
                coin = true;
                return coin;
            }
            else if (toBeContinue.Key == ConsoleKey.N)
            {
                Console.WriteLine("\n");
                Console.WriteLine("GAME OVER");
                Thread.Sleep(2000);
                return coin;
            }
            else
            {
                Console.WriteLine("\n");
                Console.WriteLine("GAME OVER");
                Thread.Sleep(2000);
                return coin;
            }
        }

        /// <summary>
        /// 플레이어의 체력바를 한눈에 볼 수 있게 출력하는 함수
        /// </summary>
        /// <param name="playerBar">플레이어의 체력을 입력받음</param>
        /// <param name="enemyBar">적의 체력을 입력받음</param>
        static void BarPrint(int playerBar, int enemyBar)
        {

            string blank = " ";

            for (int i = 0; i < playerBar; i++)
            {
                Console.Write("#");
            }
            for (int i = 0; i < 20 - playerBar; i++)
            {
                Console.Write(' ');
            }
            Console.Write("|");
            Console.Write($"{blank,-14}"); // 문자열 보간으로는 상수값만
            Console.Write("|");
            for (int i = 0; i < 20 - enemyBar; i++)
            {
                Console.Write(' ');
            }
            for (int i = 0; i < enemyBar; i++)
            {
                Console.Write("#");
            }

            return;
        }
    }

    //static void Print(Character c)
    //{
    //    //  Console.WriteLine(count);
    //    Console.WriteLine(c.name);
    //    Console.WriteLine(c.number);
    //    Console.WriteLine(c.job);
    //    Console.WriteLine(c.hp);
    //    Console.WriteLine(c.mp);

    //    foreach (Skill sk in c.skill)
    //    {
    //        Console.WriteLine($"{sk}");
    //    }

    //}
    /*남은거
     * 필살기 mp>100 일때만 나갈 수 있다, mp 최대치는 100 , mp 표시  :O
     *  ranged 장풍 데미지 + 10, 원샷캐릭터 마나 50부터 시작 : O     *  
     *  가드시 필살기 나오면 확정반격 했다고 쓰기  : O
     *  승룡을 필살기에 상쇄나게 만들면 좀 좋아지나? 승룡막으면 확정딜캐를 만들까?  O
     *  체력 바, 마나 바 를 #찍어서 구현 O
     *  플레이어, 적 캐릭터 이름 표시 o
     *  Continue? y n  o
     * 
     */
}

