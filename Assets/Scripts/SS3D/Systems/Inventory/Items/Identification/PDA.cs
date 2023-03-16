﻿using JetBrains.Annotations;
using SS3D.Interactions;
using SS3D.Interactions.Interfaces;
using SS3D.Systems.Inventory.Containers;
using SS3D.Systems.Inventory.Items.Generic;
using SS3D.Systems.Roles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SS3D.Systems.Inventory.Items.Identification
{
    /// <summary>
    /// The honking device used by the clown on honking purposes
    /// </summary>
    [RequireComponent(typeof(AttachedContainer))]
    public sealed class Pda : Item, IIdentification
    {
        /// <summary>
        /// The permission related to this PDA.
        /// </summary>
        public IDPermission Permission;

        [SerializeField]
        private AttachedContainer _attachedContainer;

        [HideInInspector] 
        public Item StartingIDCard;

        protected override void OnStart()
        {
            base.OnStart();

            _attachedContainer = GetComponent<AttachedContainer>();

            if (StartingIDCard && _attachedContainer != null)
            {
                _attachedContainer.Container.AddItem(StartingIDCard);
            }
        }

        public bool HasPermission(IDPermission permission)
        {
            if (_attachedContainer == null)
            {
                return false;
            }

            IDCard idCard = _attachedContainer.Container.Items.FirstOrDefault() as IDCard;

            if (idCard == null)
            {
                return false;
            }

            return idCard.HasPermission(permission);
        }

        [NotNull]
        public override IInteraction[] CreateTargetInteractions(InteractionEvent interactionEvent)
        {
            List<IInteraction> interactions = base.CreateTargetInteractions(interactionEvent).ToList();

            return interactions.ToArray();
        }
    }
}