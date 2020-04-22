using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IL.Simulation
{
    public class InfectionMap : MonoBehaviour
    {
        public int gridXSize;
        public int gridZSize;
        public float cellHeight = .25f;
        public float mapHeight = 3.2f;
        public GameObject block;
        public Dictionary<InfectionCell, List<GameObject>> map;

        private void Start()
        {
            map = new Dictionary<InfectionCell, List<GameObject>>();
        }

        public void AddInfection(Vector3 position)
        {
            if (map == null) return; 

            var cell = new InfectionCell(this, position);
            if (map.ContainsKey(cell))
            {
                var blocks = map[cell];
                cell.gridY = blocks.Count * cellHeight;
                Debug.Log("Block" + cell.position);
                var b = Instantiate(block, transform, false);
                b.transform.localPosition = cell.position;
                blocks.Add(b);
            }
            else
            {
                var blocks = new List<GameObject>();
                var b = Instantiate(block, transform, false);
                b.transform.localPosition = cell.position;
                blocks.Add(b);
                map.Add(cell, blocks);
            }
        }
    }

    public class InfectionCell
    {
        public int gridX;
        public float gridY;
        public int gridZ;
        private InfectionMap map;

        public InfectionCell(InfectionMap parent, Vector3 position)
        {
            gridX = Mathf.RoundToInt(position.x);
            gridY = parent.mapHeight;
            gridZ = Mathf.RoundToInt(position.z);
            map = parent;
        }

        public Vector3 position
        {
            get
            {
                return new Vector3(gridX, gridY, gridZ);
            }
        }

        public override int GetHashCode()
        {
            return gridX + gridZ * map.gridXSize;
        }
    }
}
