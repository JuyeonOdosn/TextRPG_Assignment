﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using static TextRPG_Assignment.Program;

namespace TextRPG_Assignment
{
    class Program
    {
        public class Start
        {
            public void StartScene(Warrior warrior, List<IItem> list)
            {
                if (string.IsNullOrEmpty(warrior.Name))
                {
                    Console.WriteLine("코딩너무어렵다 마을에 오신 여러분 환영합니다.\n");
                    Console.Write("이름을 설정해 주세요 :  ");
                    warrior.Name = Console.ReadLine();
                }
                Console.Clear();
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요 \n>>");

                while (true)
                {
                    string Choice = Console.ReadLine();

                    if (!int.TryParse(Choice, out int yourChoice) || (yourChoice > 3 || yourChoice < 1))
                    {

                        Console.WriteLine("잘못된 입력입니다.");
                    }
                    else
                    {
                        switch (yourChoice)
                        {
                            case 1:
                                ShowStatus(warrior, list);
                                break;
                            case 2:
                                Inventory(warrior, list);
                                break;
                            case 3:
                                Store(warrior, list);
                                break;
                        }

                        break;
                    }
                }
            }

            public void ShowStatus(Warrior warrior, List<IItem> list)
            {
                
                    Console.Clear();
                    Console.WriteLine("--상태 보기--");
                    Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
                    int pulsAttack = 0, pulsDefense = 0, pulsHealth = 0;

                    foreach (var item in warrior.EquippedItems)
                    {
                        if (item is Sword sword)
                        pulsAttack += sword.Ability(warrior);
                        else if (item is Shied shield)
                        pulsDefense += shield.Ability(warrior);
                        else if (item is Armor armor)
                        pulsHealth += armor.Ability(warrior);
                    }

                    Console.WriteLine($"Lv. {warrior.Level}");
                    Console.WriteLine($"{warrior.Name} ( 전사 )");
                    Console.WriteLine($"공격력: {warrior.Attack} (+{pulsAttack})");
                    Console.WriteLine($"방어력: {warrior.Defend} (+{pulsDefense})");
                    Console.WriteLine($"체력: {warrior.Health} (+{pulsHealth})");
                    Console.WriteLine($"Gold: {warrior.Gold} G\n");
                    Console.WriteLine();
                    Console.WriteLine("0. 나가기");
                    Console.WriteLine();
                    Console.Write("원하시는 행동을 입력해주세요 \n>>");
                
                

                while (true)
                {
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int yourInput) || yourInput != 0)
                    {
                        Console.WriteLine("0을 누르세요.");
                    }
                    else if (yourInput == 0)
                    {
                        StartScene(warrior, list);
                        break;
                    }
                }
            }

            public void Inventory(Warrior warrior, List<IItem> list)
            {
                
                    Console.Clear();
                    Console.WriteLine("--인벤토리--");
                    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                    Console.WriteLine();
                    Console.WriteLine("[아이템 목록]");

                    if (warrior.MyItems.Count > 0)
                    {
                        for (int i = 0; i < warrior.MyItems.Count; i++)
                        {
                            string equippedMark = warrior.EquippedItems.Contains(warrior.MyItems[i]) ? "[E] " : "";
                            Console.WriteLine($"- {equippedMark}{warrior.MyItems[i].Name} | {warrior.MyItems[i].AbilityText(warrior)} | {warrior.MyItems[i].Description(warrior)}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n!!보유 중인 아이템이 없습니다!!");
                    }

                    Console.WriteLine("\n \n1. 장착 관리");
                    Console.WriteLine("0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요 \n>>");
                

                while (true)
                {
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int yourInput) || (yourInput != 0 && yourInput != 1))
                    {
                        Console.WriteLine("0과 1 중 선택해 주세요.");
                    }
                    else if (yourInput == 0)
                    {
                        StartScene(warrior, list);
                        break;
                    }
                    else
                    {
                        InventoryItemSetting(warrior, list);
                        break;
                    }
                }
            }

            public void InventoryItemSetting(Warrior warrior, List<IItem> list)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착관리");
                Console.WriteLine("\n보유 중인 아이템을 관리할 수 있습니다.\n장착된 아이템은 [E]로 표시됩니다.\n\n");
                Console.WriteLine("[아이템 목록]");

                if (warrior.MyItems.Count > 0)
                {
                    for (int i = 0; i < warrior.MyItems.Count; i++)
                    {
                        string equippedMark = warrior.EquippedItems.Contains(warrior.MyItems[i]) ? "[E] " : "";
                        Console.WriteLine($"- {i+1} {equippedMark}{warrior.MyItems[i].Name} | {warrior.MyItems[i].AbilityText(warrior)} | {warrior.MyItems[i].Description(warrior)}");
                    }
                }
                else
                {
                    Console.WriteLine("\n!!보유 중인 아이템이 없습니다!!");
                }
                            
                Console.WriteLine("\n \n0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요 \n>>");

                while (true)
                {
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int yourInput) || yourInput < 0 || yourInput > warrior.MyItems.Count)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                    else if (yourInput == 0)
                    {
                        Inventory(warrior, list);
                        break;
                    }
                    else 
                    {
                        IItem selectedItem = warrior.MyItems[yourInput - 1];

                        if (warrior.EquippedItems.Contains(selectedItem))
                        {
                            warrior.EquippedItems.Remove(selectedItem);
                            InventoryItemSetting(warrior, list);
                            break;
                        }
                        else
                        {
                            bool isSameKind = false;

                            for (int i = 0; i < warrior.EquippedItems.Count; i++)
                            {
                                if (selectedItem.Kind() == warrior.EquippedItems[i].Kind())
                                {
                                    Console.WriteLine("같은 종류의 무기는 들 수 없습니다. 장착을 원하시면 기존 장비를 해제해야 합니다.");
                                    Thread.Sleep(2000);
                                    isSameKind = true;
                                    break;
                                }
                               
                            }  
                            if (!isSameKind)
                            {
                                warrior.EquippedItems.Add(selectedItem);
                                InventoryItemSetting(warrior, list);
                            }
                            InventoryItemSetting(warrior, list);

                        }
                        

                    }
                }
            }

            public void Store(Warrior warrior, List<IItem> list) 
            {
                Console.Clear();
                Console.WriteLine("상점 \n필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{warrior.Gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < list.Count; i++)
                {
                    string purchasedMark = warrior.MyItems.Contains(list[i]) ? "[구매 완료] " : $"{list[i].Money(warrior)}G";
                    Console.WriteLine($"-{list[i].Name} | {list[i].AbilityText(warrior)} | {list[i].Description(warrior)} | {purchasedMark}");
                }

                Console.WriteLine();
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요. \n>>");
                while (true)
                {
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int yourInput) || (yourInput != 0 && yourInput != 1))
                    {
                        Console.WriteLine("0과 1 중 선택해 주세요.");
                    }
                    else if (yourInput == 0)
                    {
                        StartScene(warrior, list);
                        break;
                    }
                    else
                    {
                        PurchaseItem(warrior, list);
                    }
                }
            }

            public void PurchaseItem(Warrior warrior,List<IItem> list)
            {
                Console.Clear();
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("\n[보유 골드]");
                Console.WriteLine($"{warrior.Gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < list.Count; i++)
                {
                    string purchasedMark = warrior.MyItems.Contains(list[i]) ? "[구매 완료] " : $"{list[i].Money(warrior)}G";
                    Console.WriteLine($"- {i + 1}  {list[i].Name} | {list[i].AbilityText(warrior)} | {list[i].Description(warrior)} | {purchasedMark}");
                }

                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요. \n>>");
                while (true)
                {

                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int yourInput) || (yourInput < 0 || yourInput > list.Count))
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                    }
                    else if (yourInput == 0)
                    {
                        StartScene(warrior, list);
                        break;
                    }
                    else
                    {
                        IItem wantedItem = list[yourInput - 1];
                        int pay = wantedItem.Money(warrior);

                        if (warrior.MyItems.Contains(wantedItem))
                        {
                            Console.WriteLine("이미 구매한 아이템입니다.");
                        }
                        else if (warrior.Gold >= pay)
                        {
                            warrior.Gold -= pay;
                            warrior.MyItems.Add(wantedItem);
                            Console.WriteLine("구매를 완료했습니다.");
                            Thread.Sleep(500);
                            PurchaseItem(warrior, list);
                            
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Gold가 부족합니다!");
                        }

                        
                    }
                }
            }            
        }

        public interface IItem
        {
            string Kind();
            string Name { get; set; }

            public string Description(Warrior warrior);

            public int Ability(Warrior warrior);

            public string AbilityText(Warrior warrior);

            public int Money(Warrior warrior);


        }

        public class Sword(string name, int ability, string description, int money) : IItem
        {
            public string Kind()
            {
                return "isSword";
            }
            public string Name { get; set; } = name;

            public int Ability(Warrior warrior)
            {
                return ability;
            }

            public string AbilityText(Warrior warrior)
            {
                return $"공격력 +{ability}";
            }
            public string Description(Warrior warrior)
            {
                return description;
            }
            public int Money(Warrior warrior)
            {
                return money;
            }


        }

        public class Shied(string name, int ability, string description, int money) : IItem
        {
            public string Kind()
            {
                return "isShied";
            }
            public string Name { get; set; } = name;

            public int Ability(Warrior warrior)
            {
                return ability;
            }

            public string AbilityText(Warrior warrior)
            {
                return $"방어력 +{ability}";
            }

            public string Description(Warrior warrior)
            {
                return description;
            }
            public int Money(Warrior warrior)
            {
                return money;
            }

        }

        public class Armor(string name, int ability, string description, int money) : IItem
        {
            public string Kind()
            {
                return "isArmor";
            }
            public string Name { get; set; } = name;

            public int Ability(Warrior warrior)
            {
                return ability;
            }

            public string AbilityText(Warrior warrior)
            {
                return $"체력 +{ability}";
            }

            public string Description(Warrior warrior)
            {
                return description;
            }
            public int Money(Warrior warrior)
            {
                    return money;
            }
        }
               
        public interface ICharacter
        {
            string Name { get; set; }
            int Level { get; set; }
            int Attack { get; set; }
            int Health { get; set; }
            int Defend { get; set; }
            int Gold { get; set; }

            public int TakeDamage(int damage);
        }        
        public class Warrior : ICharacter
        {
            public string Name { get; set; }
            public int Level { get; set; }
            public int Attack { get; set; }
            public int Health { get; set; }
            public int Defend { get; set; }
            public int Gold { get; set; }

            public int TakeDamage(int damage)
            {

                return damage;
            }

            public List<IItem> MyItems { get; set; } = new List<IItem>(); 
            public List<IItem> EquippedItems { get; set; } = new List<IItem>();
        }
        public static void Main(string[] args)
        {
            Warrior warrior = new Warrior();
            {
                warrior.Attack = 10;
                warrior.Health = 100;
                warrior.Defend = 5;
                warrior.Gold = 1500;
                warrior.Level = 1;
            }

            Start start = new Start();
            

            List<IItem> list = new List<IItem>
            {
            new Sword("낡은 검", 2, "쉽게 볼 수 있는 낡은 검입니다.", 600),
            new Sword("청동 도끼", 15, "어디선가 사용됐던 것 같은 도끼입니다.", 1500),
            new Sword("스파르타의 창", 24, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2000),
            new Armor("수련자 갑옷", 10, "수련에 도움을 주는 갑옷입니다.", 1000),
            new Armor("무쇠 갑옷", 22, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2200),
            new Shied("나무 방패", 2, "미미한 공격 한 번 막을까 말까한 방패입니다.", 200),
            new Shied("강철 방패", 80, "오 굉장히 단단해 보이는군요!", 10000)
            };
            start.StartScene(warrior, list);

        }
   }
 } 
