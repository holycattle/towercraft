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
				return CleanPath(ReconstructPath(closedSet, end), map, width, height);
//				return ReconstructPath(closedSet, end);
			}
			
			for (int i = 0; i < 4; i++) {
				Vector2 neighbor = new Vector2(current.pos.x + xoffs[i], current.pos.y + yoffs[i]);
				float tentative_g = current.g + Vector2.Distance(current.pos, neighbor);

				if (current.pos.x + xoffs[i] < 0)
					continue;
				if (current.pos.x + xoffs[i] >= width)
					continue;
				if (current.pos.y + yoffs[i] < 0)
					continue;
				if (current.pos.y + yoffs[i] >= height)
					continue;
				if (map[(int)current.pos.y * height + (int)current.pos.x].Tower != null) {
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
				if (found)
					continue;
				
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

	private static List<Vector2> CleanPath(List<Vector2> list, Grid[] map, int width, int height) {
		for (int i = 0; i < list.Count - 2; i++) {
			Vector2 p1 = list[i];
			Vector2 p3 = list[i + 2];


			// Source: http://playtechs.blogspot.com/2007/03/raytracing-on-grid.html
			// Note: Uses All-Integer Math Algorithm
			// Get all intersections
			float dx = Mathf.Abs((int)p3.x - (int)p1.x);
			float dy = Mathf.Abs((int)p3.y - (int)p1.y);

			int x = (int)p1.x;
			int y = (int)p1.y;

			int n = 1;
			int xIncrement, yIncrement;
			double error;

			n = 1 + (int)dx + (int)dy;
			xIncrement = p3.x > p1.x ? 1 : -1;
			yIncrement = p3.y > p1.y ? 1 : -1;
			error = dx - dy;
			dx *= 2;
			dy *= 2;

//			if (dx == 0) {
//				xIncrement = 0;
//				error = float.PositiveInfinity;
//			} else if (p3.x > p1.x) {
//				xIncrement = 1;
//				n += (int)p3.x - x;
//				error = (x + 1 - p1.x) * dy;
//			} else {
//				xIncrement = -1;
//				n += x - (int)p3.x;
//				error = (p1.x - x) * dy;
//			}
//
//			if (dy == 0) {
//				yIncrement = 0;
//				error -= float.PositiveInfinity;
//			} else if (p3.y > p1.y) {
//				yIncrement = 1;
//				n += (int)p3.y - y;
//				error -= (y + 1 - p1.y) * dx;
//			} else {
//				yIncrement = -1;
//				n += y - (int)p3.y;
//				error -= (p1.y - y) * dx;
//			}


			bool noTowersInPath = true;
			for (; n > 0; n--) {
				// Check solidity
				if (map[y * height + x].Tower != null) {
					// Solid
					noTowersInPath = false;
					break;
				}

				// Note: By manipulating the range check of [error], you can define how close you want the inteserctions
				// 	checked against the tower pieces.
				if (error > 0) {
					x += xIncrement;
					error -= dy;
				} else if (error < 0) {
					y += yIncrement;
					error += dx;
				} else {
//					if (y + 1 < height && map[(y + 1) * height + x].Tower != null ||
//						x + 1 < width && map[y * height + (x + 1)].Tower != null) {
//						// Solid
//						noTowersInPath = false;
//						break;
//					}

					// TODO: Still not perfect.
					if (y + 1 < height && map[(y + 1) * height + x].Tower != null ||
						x + 1 < width && map[y * height + (x + 1)].Tower != null ||
						y - 1 >= 0 && map[(y - 1) * height + x].Tower != null ||
						x - 1 >= 0 && map[y * height + (x - 1)].Tower != null) {
						// Solid
						noTowersInPath = false;
						break;
					}
				}
			}

			if (noTowersInPath) {
				// Remove p2
				list.RemoveAt(i + 1);
				i--;
			} else {
//				Debug.Log("No path!");
			}
		}
		return list;
	}
}

class Node {
	public Node parent;
	public Vector2 pos;
	public float g;
	public float f;
	
	public Node (Vector2 pos) {
		this.pos = pos;
		g = 0;
		f = 0;
		parent = null;
	}
	
	public Node (Vector2 pos, float g, float f) {
		this.pos = pos;
		this.g = g;
		this.f = f;
		parent = null;
	}
}