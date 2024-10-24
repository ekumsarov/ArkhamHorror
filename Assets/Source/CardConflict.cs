using System.Collections;
using EVI.DDSystem;
using UnityEngine;

namespace EVI
{
    public class CardConflict : MonoBehaviour
    {
        public void ConflictedCards(GameCardView movingCard, GameCardView cellCard)
        {
            // Сначала проверяем типы карт по их CardType enum
            var movingType = movingCard.Model.CardType;
            var cellType = cellCard.Model.CardType;

            switch (movingType)
            {
                case CardType.Player when cellType == CardType.Player:
                    HandlePlayerVsPlayer(movingCard, cellCard);
                    break;

                case CardType.Enemy when cellType == CardType.Enemy:
                    HandleMonsterVsMonster(movingCard, cellCard);
                    break;

                case CardType.Item when cellType == CardType.Item:
                    HandleItemVsItem(movingCard, cellCard);
                    break;

                case CardType.Player when cellType == CardType.Enemy:
                    HandlePlayerVsMonster(movingCard, cellCard);
                    break;

                case CardType.Enemy when cellType == CardType.Player:
                    HandleMonsterVsPlayer(movingCard, cellCard);
                    break;

                case CardType.Item when cellType == CardType.Player:
                    HandleItemToPlayer(movingCard, cellCard);
                    break;

                case CardType.Item when cellType == CardType.Enemy:
                    HandleItemToMonster(movingCard, cellCard);
                    break;

                default:
                    HandleDefaultConflict(movingCard, cellCard);
                    break;
            }
        }

        // Пример методов для обработки конфликтов

        private void HandlePlayerVsPlayer(GameCardView card1, GameCardView card2)
        {
            Debug.Log("Player switch player!");
            
            Slot movingSlot = card1.CurrentSlot;
            Slot baseSlot = card2.CurrentSlot;

            baseSlot.RemoveItem(card2);

            baseSlot.TryPlaceItem(card1);
            movingSlot.TryPlaceItem(card2);
        }

        private void HandleMonsterVsMonster(GameCardView card1, GameCardView card2)
        {
            Debug.Log("Two monsters fight each other!");
            // Логика боя между двумя монстрами
        }

        private void HandleItemVsItem(GameCardView card1, GameCardView card2)
        {
            Debug.Log("Two items collide. Maybe they merge?");
            // Логика взаимодействия двух предметов (например, объединение)
        }

        private void HandlePlayerVsMonster(GameCardView player, GameCardView monster)
        {
            Debug.Log("Player fights a monster!");
            
            int playerHealthDamage = player.Model.HealthDamage;
            int monsterHealthDamage = monster.Model.HealthDamage;
            int monsterMindDamage = monster.Model.MindDamage;

            player.Model.DamageHealth(monsterHealthDamage);
            player.Model.DamageMind(monsterMindDamage);
            monster.Model.DamageHealth(playerHealthDamage);

            player.SetDraggable(false);
        }

        private void HandleMonsterVsPlayer(GameCardView monster, GameCardView player)
        {
            Debug.Log("Monster attacks the player!");
            // Логика атаки монстра на игрока
        }

        private void HandleItemToPlayer(GameCardView item, GameCardView player)
        {
            Debug.Log("Player picks up an item.");
            // Логика передачи предмета игроку
        }

        private void HandleItemToMonster(GameCardView item, GameCardView monster)
        {
            Debug.Log("Monster interacts with the item.");
            // Логика взаимодействия монстра с предметом
        }

        private void HandleDefaultConflict(GameCardView card1, GameCardView card2)
        {
            Debug.Log("Handling default conflict.");
            // Общая логика для случаев, не охваченных другими правилами
        }
    }
}
