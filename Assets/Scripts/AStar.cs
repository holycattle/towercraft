using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {
	public static List<Vector2> PathFind(Vector2 start, Vector2 end, Grid[] map, int width, int height) {
		int[] xoffs = {-1, 0, 1, 0, -1, 1, 1, -1};
		int[] yoffs = {0, -1, 0, 1, 1, 1, -1, -1};
		
		List<Node> closedSet = new List<Node>();
		List<Node> openSet = new List<Node>();
		
		// Add the initial state.
		openSet.Add(new Node(start, 0f, Vector2.Distance(new Vector2(0, 0), start)));
		
		while (openSet.Count > 0) {
			// Find the Node with the lowest F
			int lowestIndex = 0;
			for (int i = 1; i < openSet.Count; i++) {
				if (openSet[i].f < openSet[lowestIndex].f) {
					lowestIndex = i;
				}
			}
			
			Node current = openSet[lowestIndex];
			openSet.Remove(current);
			closedSet.Add(current);
			
			if (current.pos == end) {
				return ReconstructPath(closedSet, end);
			}
			
			for (int i = 0; i < 4; i++) {
				Vector2 neighbor = new Vector2(current.pos.x + xoffs[i], current.pos.y + yoffs[i]);
				float tentative_g = current.g + Vector2.Distance(current.pos, neighbor);
				
				if (current.pos.x + xoffs[i] < 0) continue;
				if (current.pos.x + xoffs[i] >= width) continue;
				if (current.pos.y + yoffs[i] < 0) continue;
				if (current.pos.y + yoffs[i] >= height) continue;
				if (map[(int) current.pos.y * height + (int) current.pos.x].Tower != null) {
					continue;
				}
				
				// Check if the node has already been visited
				bool found = false;
				for (int j = 0; j < closedSet.Count; j++) {
					if (closedSet[j].pos == neighbor) {
						found = true;
						break;
					}
				}
				if (found) continue;
				
				// Check if neighbor is in openSet
				bool inOpenSet = false;
				int o = 0;
				for (o = 0; o < openSet.Count; o++) {
					if (openSet[o].pos == neighbor) {
						inOpenSet = true;
						break;
					}
				}
				
				Node n;
				if (!inOpenSet || tentative_g < current.g) {
					if (!inOpenSet) {
						n = new Node(neighbor, tentative_g, tentative_g + Vector2.Distance(neighbor, end));
						openSet.Add(n);
					} else {
						n = openSet[o];
					}
					n.parent = current;
					n.g = tentative_g;
					n.f = tentative_g + Vector2.Distance(neighbor, end);
				}
			}
		}
		return null;
	}
	
	private static List<Vector2> ReconstructPath(List<Node> list, Vector2 target) {		
		List<Vector2> solution = new List<Vector2>();
		
		// Find index of last.
		int index = 0;
		for (int i = 0; i < list.Count; i++) {
			if (list[i].pos == target) {
				index = i;
				break;
			}
		}
		Node current = list[index];
		
		while (current.parent != null) {
			solution.Insert(0, current.pos);
			current = current.parent;
		}
		solution.Insert(0, current.pos);
				
		return solution;
	}
}

class Node {
	public Node parent;
	public Vector2 pos;
	public float g;
	public float f;
	
	public Node(Vector2 pos) {
		this.pos = pos;
		g = 0;
		f = 0;
		parent = null;
	}
	
	public Node(Vector2 pos, float g, float f) {
		this.pos = pos;
		this.g = g;
		this.f = f;
		parent = null;
	}
}