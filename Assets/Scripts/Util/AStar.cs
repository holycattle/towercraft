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
//				return CleanPath(ReconstructPath(closedSet, end), map, width, height);
				return CleanPathNew(ReconstructPath(closedSet, end), map, width, height);
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

	public const int TILE_SIZE = 8;
	public const int HTILE_SIZE = TILE_SIZE / 2;
	public static int startXPosition = -(16 / 2) * TILE_SIZE;
	public static int startYPosition = -(16 / 2) * TILE_SIZE;

	private static void DRAW(List<Vector2> list) {
		// Draw Smoothed Path
		List<Vector2> act = new List<Vector2>();
		for (int i = 0; i < list.Count; i++) {
			act.Add(new Vector2(startXPosition + list[i].x * TILE_SIZE + HTILE_SIZE, startYPosition + list[i].y * TILE_SIZE + HTILE_SIZE));
		}
		for (int i = 0; i < act.Count - 1; i++) {
			Vector3 p = new Vector3(act[i].x, 1, act[i].y);

			Debug.DrawLine(p, new Vector3(act[i + 1].x, 1, act[i + 1].y), Color.red);
			Debug.DrawLine(p, p + Vector3.up * 5, Color.red);
		}
	}

	private static List<Vector2> CleanPathNew(List<Vector2> list, Grid[] map, int width, int height) {
		for (int i = 0; i < list.Count - 2; i++) {
			Vector2 p1 = list[i];
			p1.x = p1.x + 0.5f;
			p1.y = p1.y + 0.5f;
			Vector2 p3 = list[i + 2];
			p3.x = p3.x + 0.5f;
			p3.y = p3.y + 0.5f;

			bool noTowersInPath = true;

			// Get slope
			Vector2 slope = p3 - p1;
			slope.Normalize();
			Vector2 negSlope = new Vector2(-slope.y, slope.x);
			negSlope.Normalize();
			negSlope = negSlope * 0.2f;
			slope = slope * 0.1f;

			Vector2 leftLine = p1 - negSlope;
			Vector2 rightLine = p1 + negSlope;

			float dist = Vector2.Distance(p1, p3);
			for (; dist >= 0; dist -= 0.1f) {
				if ((int)leftLine.y < height && (int)leftLine.y >= 0 &&
					(int)leftLine.x < width && (int)leftLine.x >= 0 &&
					map[(int)leftLine.y * height + (int)leftLine.x].Tower != null) {
					noTowersInPath = false;
					break;
				}
				if ((int)rightLine.y < height && (int)rightLine.y >= 0 &&
					(int)rightLine.x < width && (int)rightLine.x >= 0 &&
					map[(int)rightLine.y * height + (int)rightLine.x].Tower != null) {
					noTowersInPath = false;
					break;
				}

				leftLine += slope;
				rightLine += slope;
			}

			if (noTowersInPath) {
				// Remove p2
				list.RemoveAt(i + 1);
				i--;
			} else {
//				Debug.Log("No path!");
			}
		}

		List<Vector2> act = new List<Vector2>();

		// If there are no towers that obstruct the path,
		// then the only 2 points on the path are START and END
		// ELSE: Do standard procedure.
		if (list.Count == 2) {
			act.Add(new Vector2(0.5f, -0.5f));					// Start.
			act.Add(list[0] + new Vector2(0.5f, 0.5f));			// Start Grid Intersection.
			act.Add(list[1] + new Vector2(0.5f, 0.5f));			// End Grid Intersection.
			act.Add(new Vector2(width - 0.5f, height + 0.5f));	// End.
		} else {
			list.Insert(0, new Vector2(0f, -1f));
			list.Add(new Vector2(width - 1, height));

			Debug.Log("Length: " + list.Count);
			for (int i = 1; i < list.Count - 1; i++) {
				Vector2 p1 = list[i - 1];
				p1.x = p1.x + 0.5f;
				p1.y = p1.y + 0.5f;
				Vector2 p2 = list[i];
				p2.x = p2.x + 0.5f;
				p2.y = p2.y + 0.5f;
				Vector2 p3 = list[i + 1];
				p3.x = p3.x + 0.5f;
				p3.y = p3.y + 0.5f;

				Vector2 np1, np2;
				Vector2 temp = p1 - p2;
				if (Mathf.Abs(temp.x) < Mathf.Abs(temp.y)) {
					// Hits the Y-Axis
					np1 = p2 + new Vector2((0.5f * temp.x * Mathf.Sign(temp.y)) / temp.y, Mathf.Sign(temp.y) * 0.5f);
				} else {
					// Hits the X-Axis
					np1 = p2 + new Vector2(Mathf.Sign(temp.x) * 0.5f, (0.5f * temp.y * Mathf.Sign(temp.x)) / temp.x);
				}

				temp = p3 - p2;
				if (Mathf.Abs(temp.x) < Mathf.Abs(temp.y)) {
					// Hits the Y-Axis
					np2 = p2 + new Vector2((0.5f * temp.x * Mathf.Sign(temp.y)) / temp.y, Mathf.Sign(temp.y) * 0.5f);
				} else {
					// Hits the X-Axis
					np2 = p2 + new Vector2(Mathf.Sign(temp.x) * 0.5f, (0.5f * temp.y * Mathf.Sign(temp.x)) / temp.x);
				}

				if (i == 1)
					act.Add(p1);
				act.Add(np1);
				act.Add(np2);
				if (i == list.Count - 2)
					act.Add(p3);
			}
		}
		return act;
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