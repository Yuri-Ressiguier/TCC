using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [field: SerializeField] public List<Door> Doors { get; set; }
    [field: SerializeField] public List<Spawner> Spawners { get; set; }


    // Update is called once per frame
    void Update()
    {
        var hasMonsters = Spawners.Any(x => x.CanRespawn == true);
        if (!hasMonsters)
        {
            Doors.ForEach(x => x.Open());
            Destroy(gameObject);
        }
    }
}
