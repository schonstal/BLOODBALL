using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Dodgeball.Engine {
  static class Util {
    public static float computeVelocity(
        float velocity, float acceleration=0, float drag=0, float max=9000, int steps=1) {
			if(acceleration != 0)
				velocity += acceleration * G.elapsed/steps;
			else if(drag != 0)
			{
				float relativeDrag = drag * G.elapsed/steps;
				if(velocity - relativeDrag > 0)
					velocity -= relativeDrag;
				else if(velocity + relativeDrag < 0)
					velocity += relativeDrag;
				else
					velocity = 0;
			}
      if(max > 0) {
        if(velocity > max)
          velocity = max;
        else if(velocity < -max)
          velocity = -max;
      }
			return velocity;
		}

    public static bool Overlaps(GameObject objectOne, GameObject objectTwo) {
      //Do fancy collision later
      /*bool end = true;
      if(typeof(Group) == objectOne.GetType()) {
        List<GameObject> group = ((Group)objectOne).Members;
        foreach(GameObject o in group) {
          group.Remove(o);
          Overlaps(o, objectTwo, callback);
        }
        end = false;
      }
      if(typeof(Group) == objectOne.GetType()) {
        ((Group)objectTwo).Members.ForEach((o) => Overlaps(o, objectTwo, callback));
        end = false;
      }
      if(end)*/

      if(objectOne.Hitbox.Intersects(objectTwo.Hitbox)) {
        return true;
      } else {
        return false;
      }
    }
  }
}