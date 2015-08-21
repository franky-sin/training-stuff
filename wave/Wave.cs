using System;
//using System.Math;
using System.IO;
using System.Collections.Generic;


class Hunter{
	bool knows_victim_location
}

abstract class Char {
	
}

class Program
{
	static Dictionary<Char, Boolean> symbols{
		get{
			Dictionary<Char, Boolean> res = new Dictionary<Char, Boolean>();
			res.Add('b', false);
			res.Add('#', false);
			res.Add(' ', true);
			res.Add('s', true);
			res.Add('f', true);
			res.Add('w', true);
			return res;
		}
	}

	static char[][] testArr {
		get{
			return new char[10][]{
				new char[10] {'s', ' ', 'b', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
				new char[10] {' ', ' ', 'b', 'b', ' ', ' ', ' ', ' ', ' ', ' '},
				new char[10] {'b', 'b', ' ', 'b', ' ', ' ', ' ', ' ', 'b', ' '},
				new char[10] {' ', ' ', ' ', 'b', ' ', ' ', ' ', ' ', 'b', ' '},
				new char[10] {' ', 'b', 'b', 'b', 'b', 'b', 'b', ' ', 'b', ' '},
				new char[10] {' ', ' ', 'b', 'b', ' ', 'b', ' ', 'b', 'b', ' '},
				new char[10] {' ', 'b', ' ', 'b', ' ', 'b', ' ', 'b', ' ', 'b'},
				new char[10] {' ', 'b', ' ', 'b', 'b', ' ', 'b', 'b', 'b', ' '},
				new char[10] {' ', ' ', ' ', ' ', ' ', 'b', ' ', 'b', ' ', ' '},
				new char[10] {' ', ' ', ' ', 'b', ' ', 'b', ' ', 'b', ' ', 'f'},
				
			};
		}
		// set;
	}

	static KeyValuePair<int,int> start = new KeyValuePair<int,int>(0,0);
	static KeyValuePair<int,int> finish = new KeyValuePair<int,int>(1,7);

	// static char[][] formArray(int X, int Y, KeyValuePair<int,int> start){
	// 	if (X <= start.Key || Y <= start.Value){
	// 		return null;
	// 	}
	// 	char[][] result = new char[X][Y];
	// 	for(int i = 0; i < X; i++){
	// 		for(int i = 0; i < X; i++){

	// 		}
	// 	}
	// 	return result;
	// }

	static void locateStartAndFinish( char[][] map ){
		for(int i = 0; i < map.Length; i++){
			if(-1 != Array.IndexOf(map[i], 's')){
				start = new KeyValuePair<int, int>(i, Array.IndexOf(map[i], 's'));
				break;
			}
		}

		for(int i = 0; i < map.Length; i++){
			if(-1 != Array.IndexOf(map[i], 'f')){
				finish = new KeyValuePair<int, int>(i, Array.IndexOf(map[i], 'f'));
				break;
			}
		}
	}

	static List<KeyValuePair<int,int>> findWay (char[][] map) {
		//char[][]
		locateStartAndFinish(map);
		int?[][] pathFinder = new int?[map.Length][];
		for(int i = 0; i < map.Length; i++){
			pathFinder[i] = new int?[map[i].Length];
			for(int j = 0; j < map[i].Length; j++){
				pathFinder[i][j] = null;
			}
		}
		// Console.WriteLine("pathFinder last >> " + pathFinder[6][6]);
		List<KeyValuePair<int,int>> path = new List<KeyValuePair<int,int>>();
		pathFinder[start.Key][start.Value] = 0;
		map[start.Key][start.Value] = 's';
		map[finish.Key][finish.Value] = 'f';

		int d = 0;
		Boolean finishFound = false;
		// Boolean waveHasWay = true;
		List<KeyValuePair<int,int>> currentPoints = new List<KeyValuePair<int,int>>();
		currentPoints.Add(new KeyValuePair<int,int>(start.Key, start.Value));
		while (!finishFound && 0 != currentPoints.Count){
			List<KeyValuePair<int,int>> temp = new List<KeyValuePair<int,int>>();
			foreach(KeyValuePair<int,int> p in currentPoints){
				for(int i = (p.Key-1); i <= (p.Key+1); i++){
					if(pathFinder.Length <= i || 0 > i){
						continue;
					}
					for(int j = (p.Value-1); j <= (p.Value+1); j++){
						if(i == p.Key && j == p.Value){
							continue;
						}
						if(pathFinder[i].Length <= j || 0 > j){
							continue;
						}
						// Boolean o = true;
						// Console.WriteLine("map[i][j] >> |" + map[i][j] + "|");
						// Console.WriteLine("symbols.TryGetValue(map[i][j], out o) >> " + symbols.TryGetValue(map[i][j], out o));
						if(!symbols[map[i][j]]){
						// if('b' == map[i][j]){
							continue;
						}
						if (null == pathFinder[i][j]) {
							pathFinder[i][j] = d + 1;
							temp.Add(new KeyValuePair<int,int>(i, j));
							if(finish.Key == i && finish.Value == j){
								finishFound = true;
							}
						}
					}
				}
			}
			currentPoints = new List<KeyValuePair<int,int>>();
			currentPoints = temp;
			d++;
		}
		
		// Console.WriteLine("finishFound >> " + finishFound);
		Boolean startFound = false;
		d = (int)pathFinder[finish.Key][finish.Value];
		if(finishFound){
			KeyValuePair<int,int> p = new KeyValuePair<int,int>();
			p = finish;
			path.Add(finish);
			KeyValuePair<int,int> temp = new KeyValuePair<int,int>();
			while(!startFound){
				bool gotchaEarly = false;
				if (d == 23){
					Console.WriteLine("p.Value-1 >> " + (p.Value-1) );
				}
				if(0 <= (p.Key-1) && !gotchaEarly){
					if( (d-1) == pathFinder[p.Key-1][p.Value] ){
						temp = new KeyValuePair<int,int>(p.Key-1, p.Value);
						path.Add(temp);
						gotchaEarly = true;
					}
				}
				if(pathFinder.Length > (p.Key+1) && !gotchaEarly){
					if( (d-1) == pathFinder[p.Key+1][p.Value] ){
						temp = new KeyValuePair<int,int>(p.Key+1, p.Value);
						path.Add(temp);
						gotchaEarly = true;
					}
				}
				if(pathFinder[p.Key].Length > (p.Value+1) && !gotchaEarly){
					if( (d-1) == pathFinder[p.Key][p.Value+1] ){
						temp = new KeyValuePair<int,int>(p.Key, p.Value+1);
						path.Add(temp);
						gotchaEarly = true;
					}
				}
				if(0 <= (p.Value-1) && !gotchaEarly){
					// if (d == 23){
					// 	Console.WriteLine("p.Value-1 >> " + (p.Value-1) );
					// }
					if( (d-1) == pathFinder[p.Key][p.Value-1] ){
						temp = new KeyValuePair<int,int>(p.Key, p.Value-1);
						path.Add(temp);
						gotchaEarly = true;
					}
				}

				if(0 != path.Count){
					if(start.Key == path[(path.Count-1)].Key && start.Value == path[(path.Count - 1)].Value){
						startFound = true;
						path.RemoveAt(path.Count-1);
					}
				}

				if(!gotchaEarly){
					for(int i = (p.Key-1); i <= (p.Key+1); i++){
						if(pathFinder.Length <= i || 0 > i){
							continue;
						}
						Boolean gotcha = false;
						for(int j = (p.Value-1); j <= (p.Value+1); j++){
							// Console.WriteLine("i >> " + i);
							// Console.WriteLine("j >> " + j);
							if(pathFinder[i].Length <= j || 0 > j){
								continue;
							}
							if(start.Key == i && start.Value == j){
								startFound = true;
								temp = new KeyValuePair<int,int>(i, j);
								path.Add(temp);
								gotcha = true;
							}else{
								if( (d-1) == pathFinder[i][j] ){
									temp = new KeyValuePair<int,int>(i, j);
									path.Add(temp);
									gotcha = true;
								}
							}
							if(gotcha){
								break;
							}
						}
						if(gotcha){
							break;
						}
					}
				}


				if(!startFound){
					p = temp;
					d--;
				}
			}
		}else{
			// return map;
			return null;
		}

		return path;

		Console.WriteLine("Path Finder >> ");
		for (int i =0; i < pathFinder.Length; i++){
			Console.Write("|");
			for(int j =0; j < pathFinder[i].Length; j++){
				if(null == pathFinder[i][j]){
					Console.Write(" ");
				}else{
					Console.Write(pathFinder[i][j]);
				}
				Console.Write("|");
			}
			Console.WriteLine();
		}

		foreach(KeyValuePair<int,int> p in path){
			map[p.Key][p.Value] = '@';
		}
		// return map;
		return null;
	}

	static void Main(string[] args)
	{
		char [][] map;
		String[] temp = File.ReadAllLines("maze.txt");
		map = new char[temp.Length][];
		for(int i = 0; i < temp.Length; i++){
			map[i] = temp[i].ToCharArray();
		}

		for (int i =0; i < map.Length; i++){
			// Console.Write("|");
			for(int j =0; j < map[i].Length; j++){
				if('w' == map[i][j]){
					Console.Write(' ');
				}else{
					Console.Write(map[i][j]);
				}
				// Console.Write("|");
			}
			Console.WriteLine();
		}

		// foreach(KeyValuePair<char,Boolean> p in symbols){
		// 	Console.WriteLine(p.Key + " >> " + p.Value);
		// }

		// char[][] res = findWay(map);
		List<KeyValuePair<int,int>> res = findWay(map);
		Console.WriteLine("result >> ");

		for(int k = (res.Count-1); k >= 0; k--){
			for (int i =0; i < map.Length; i++){
				// Console.Write("|");
				for(int j =0; j < map[i].Length; j++){
					if(i == res[k].Key && j == res[k].Value){
						Console.Write('@');
					}else if('w' == map[i][j]){
						Console.Write(' ');
					}else{
						Console.Write(map[i][j]);
					}
					// Console.Write(res[i][j]);
					// Console.Write("|");
				}
				Console.WriteLine();
			}
			Console.ReadLine();
			if(0 != k){
				Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop-map.Length-1);
			}
		}
		return;
	}
}