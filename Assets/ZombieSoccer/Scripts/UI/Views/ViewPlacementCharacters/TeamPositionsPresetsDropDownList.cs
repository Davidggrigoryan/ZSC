using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Zenject;

namespace ZombieSoccer.GameLayer.UI
{
    public class TeamPositionsPresetsDropDownList : MonoBehaviour
    {
        [SerializeField]
        private DropDownList dropDownList;

        [Inject]
        public TeamPositionsPresetsCollection positionsPresets;

        private void Start()
        {
            dropDownList = GetComponent<DropDownList>();
            SetTeamPositionsPresetsIntoDropDownList();
        }

        private void SetTeamPositionsPresetsIntoDropDownList()
        {
            foreach (var pos in positionsPresets.teamPositions)
            {
                dropDownList.AddItem(pos.ToString());
            }
        }
    }
}
