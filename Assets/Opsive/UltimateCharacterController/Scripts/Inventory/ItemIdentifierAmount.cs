/// ---------------------------------------------
/// Ultimate Character Controller
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateCharacterController.Inventory
{
    using Opsive.Shared.Inventory;
    using UnityEngine;

    /// <summary>
    /// Specifies the amount of each ItemDefinitionBase that the character can pickup or is loaded with the default inventory.
    /// </summary>
    [System.Serializable]
    public class ItemDefinitionAmount
    {
        [Tooltip("The type of item.")]
        [UnityEngine.Serialization.FormerlySerializedAs("m_ItemType")]
        [SerializeField] protected ItemDefinitionBase m_ItemDefinition;
        [Tooltip("The number of ItemIdentifier units to pickup.")]
        [UnityEngine.Serialization.FormerlySerializedAs("m_Count")]
        [SerializeField] protected int m_Amount = 1;

        private IItemIdentifier m_ItemIdentifier;

        public ItemDefinitionBase ItemDefinition { get { return m_ItemDefinition; } set { m_ItemDefinition = value; } }
        public int Amount { get { return m_Amount; } set { m_Amount = value; } }

        public IItemIdentifier ItemIdentifier { 
            get {
                if (Application.isPlaying && m_ItemIdentifier == null) {
                    m_ItemIdentifier = m_ItemDefinition.CreateItemIdentifier();
                }
                return m_ItemIdentifier;
            } 
        }

        /// <summary>
        /// ItemDefinitionAmount constructor with no parameters.
        /// </summary>
        public ItemDefinitionAmount() { }

        /// <summary>
        /// ItemDefinitionAmount constructor with two parameters.
        /// </summary>
        /// <param name="itemDefinition">The definition of item.</param>
        /// <param name="amount">The amount of ItemDefinitionBase.</param>
        public ItemDefinitionAmount(ItemDefinitionBase itemDefinition, int amount)
        {
            Initialize(itemDefinition, amount);
        }

        /// <summary>
        /// Initializes the ItemDefinitionAmount to the specified values.
        /// </summary>
        /// <param name="itemIdentifier">The definition of item.</param>
        /// <param name="amount">The amount of ItemDefinitionBase.</param>
        public void Initialize(ItemDefinitionBase itemDefinition, int amount)
        {
            m_ItemDefinition = itemDefinition;
            m_Amount = amount;
        }
    }
}