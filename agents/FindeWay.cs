using System;
//using System.Math;
using System.IO;
using System.Collections.Generic;

class Program
{
	class Agent {
		const int sight_const = 2;										// How far agent sees
		List<KeyValuePair<int,int>> memory;					// Squares, that agent has already been on
		public Dictionary<KeyValuePair<int,int>, bool> i_see;		// Squares, that agent sees
		// char [][] i_see_map;								// Maybe a map of squares he sees?
		public KeyValuePair<int,int> pos;							// Current agent's position
		bool HasChoice{										// Agent has a choice he has never made before
			get{
				return 0 != choices.Count;
			}
		}
		public List<KeyValuePair<int,int>> choices;				// List of choices
		int TraceBack; 										// Used to trace back the way if agent in a deadlock

		bool SeesExit;										// Agent sees exit from maze

		public char sym;
		public String Name;

		String[] directions = new String[]{"u","d","l","r"};
		// ,"ur","ul","dr","dl"

		public KeyValuePair<int,int> dist {get; private set;}

		public Agent(char sym, String Name){
			this.sym = sym;
			this.Name = Name;
			memory = new List<KeyValuePair<int,int>>();
			TraceBack = memory.Count - 1;
		}

		public void doLive(){
			try{
				Console.WriteLine("dist before >> " + dist);
				Console.WriteLine("pos before >> " + pos);
				GoToPos(dist);											// Agent goes to new position
				memory.Add(dist);
				Console.WriteLine("pos after >> " + pos);
			}catch(Exception E){

			}
			if(null != i_see){
				i_see.Clear();
				// Console.WriteLine("i see clear");
			}
			if(null != choices){
				// Console.WriteLine("choices clear");
				choices.Clear();
			}
			i_see = new Dictionary<KeyValuePair<int,int>, bool>();
			// Console.WriteLine("i see size {0}", i_see.Count);
			choices = new List<KeyValuePair<int,int>> ();
			LookAround();									// Agent reads data of nearby objects
			dist = MakeDecision();	// Agent makes decision where to go, and remembers current position
		}

		private void GoToPos( KeyValuePair<int,int> dist ){
			List<KeyValuePair<int,int>> way = findWay(pos, dist);
			pos = way[way.Count-1];
		}

		private KeyValuePair<int,int> MakeDecision(){
			// KeyValuePair<int,int> dist;
			// Console.WriteLine("KeyValuePair<int,int> dist;");
			// Console.WriteLine("HasChoice >> " + HasChoice);
			// Console.WriteLine("i_see >> {0}" , i_see.Count);
			// if(8 <= this.i_see.Count){
			// 	Console.WriteLine("if(8 <= i_see.Count){");
			// 	dist = pos;
			// }else 
			if(HasChoice){
				// Console.WriteLine("}else if(HasChoice){");
				TraceBack = memory.Count - 1;
				// Console.WriteLine("TraceBack = memory.Count - 1;");
				Random rnd = new Random();
				int index = rnd.Next(0, choices.Count);
				dist = choices[index];
				memory.Add(pos);
			}else if(-1 != TraceBack){
				// Console.WriteLine("}else if(-1 != TraceBack){");
				dist = memory[TraceBack];
				TraceBack--;
			}else{
				Console.WriteLine("}else{ dist = pos");
				dist = pos;
			}

			return dist;
		}

		public void LookAround(){
			// Remember();
			foreach(String d in directions){
				LookDirection(d);
			}
			Console.WriteLine("pos = {0} : {1}", pos.Key, pos.Value);
			for(int corners = 1; corners <= 4; corners++){
				KeyValuePair<int, int> looking_at;
				String[] temp_dirs = new String[]{"u","l","ul"};
				int x = 0, y = 0;

				if(1 == corners)
				{ 
					x = -1; 
					y = -1; 
					temp_dirs = new String[]{"u","l","ul"}; 
				}
				else if(2 == corners)
				{ 
					x = 1; 
					y = -1; 
					temp_dirs = new String[]{"u","r","ur"}; 
				}
				else if(3 == corners)
				{ 
					x = -1; 
					y = 1; 
					temp_dirs = new String[]{"d","l","dl"}; 
				}
				else if(4 == corners)
				{ 
					x = 1; 
					y = 1; 
					temp_dirs = new String[]{"d","r","dr"}; 
				}

				looking_at = new KeyValuePair<int,int>(pos.Key+y, pos.Value+x);
				bool in_Map = map.Length > looking_at.Key && 0 <= looking_at.Key && map[looking_at.Key].Length > looking_at.Value && 0 <= looking_at.Value;
				if(in_Map){
					// Console.WriteLine("corners {0}", corners);
					// Console.WriteLine("{0} : {1}",looking_at.Key, looking_at.Value);
					// Console.WriteLine("i see add");
					i_see.Add(looking_at, symbols[map[looking_at.Key][looking_at.Value]]);
					bool Blocked = !symbols[map[looking_at.Key][looking_at.Value]];
					// Console.WriteLine("Blocked {0}", Blocked);
					if(!Blocked){
						foreach(String d in temp_dirs){
							LookDirection(d, looking_at, 1);
						}
					}
				}
			}
		}

		private void LookDirection(String  direction, int sight = sight_const){
			bool Blocked = false;
			for(int look = 1; look <= sight; look++){
				KeyValuePair<int, int> looking_at;
				switch (direction)
				{
					case "u":
						looking_at = new KeyValuePair<int,int>(pos.Key-look, pos.Value);
						break;
					case "d":
						looking_at = new KeyValuePair<int,int>(pos.Key+look, pos.Value);
						break;
					case "r":
						looking_at = new KeyValuePair<int,int>(pos.Key, pos.Value+look);
						break;
					case "l":
						looking_at = new KeyValuePair<int,int>(pos.Key, pos.Value-look);
						break;
					case "ul":
						looking_at = new KeyValuePair<int,int>(pos.Key-look, pos.Value-look);
						break;
					case "ur":
						looking_at = new KeyValuePair<int,int>(pos.Key-look, pos.Value+look);
						break;
					case "dl":
						looking_at = new KeyValuePair<int,int>(pos.Key+look, pos.Value-look);
						break;
					case "dr":
						looking_at = new KeyValuePair<int,int>(pos.Key+look, pos.Value+look);
						break;
					default:
						looking_at = new KeyValuePair<int,int>(pos.Key, pos.Value);
						break;
				}
				bool in_Map = map.Length > looking_at.Key && 0 <= looking_at.Key && map[looking_at.Key].Length > looking_at.Value && 0 <= looking_at.Value;
				// Console.WriteLine("in_Map >> " + in_Map);
				if(in_Map){
					// Console.WriteLine("map[looking_at.Key][looking_at.Value] >> " + map[looking_at.Key][looking_at.Value]);
					// Console.WriteLine("{0} : {1}",looking_at.Key, looking_at.Value);
					// Console.WriteLine("i see add");
					i_see.Add(looking_at, symbols[map[looking_at.Key][looking_at.Value]]);
					Blocked = !symbols[map[looking_at.Key][looking_at.Value]];
					// Console.WriteLine("look >> " + look);
					// Console.WriteLine("Blocked >> " + Blocked);
					if(sight == look && !Blocked && !memory.Contains(looking_at) && !choices.Contains(looking_at)){
						// -1 == Array.IndexOf(memory, looking_at)
						// Console.WriteLine("choices add");
						choices.Add(looking_at);
					}
					if(Blocked){
						break;
					}
				}else{
					break;
				}
			}
		}

		private void LookDirection(String  direction, KeyValuePair<int,int> point_look, int sight = sight_const){
			bool Blocked = false;
			for(int look = 1; look <= sight; look++){
				KeyValuePair<int, int> looking_at;
				switch (direction)
				{
					case "u":
						looking_at = new KeyValuePair<int,int>(point_look.Key-look, point_look.Value);
						break;
					case "d":
						looking_at = new KeyValuePair<int,int>(point_look.Key+look, point_look.Value);
						break;
					case "r":
						looking_at = new KeyValuePair<int,int>(point_look.Key, point_look.Value+look);
						break;
					case "l":
						looking_at = new KeyValuePair<int,int>(point_look.Key, point_look.Value-look);
						break;
					case "ul":
						looking_at = new KeyValuePair<int,int>(point_look.Key-look, point_look.Value-look);
						break;
					case "ur":
						looking_at = new KeyValuePair<int,int>(point_look.Key-look, point_look.Value+look);
						break;
					case "dl":
						looking_at = new KeyValuePair<int,int>(point_look.Key+look, point_look.Value-look);
						break;
					case "dr":
						looking_at = new KeyValuePair<int,int>(point_look.Key+look, point_look.Value+look);
						break;
					default:
						looking_at = new KeyValuePair<int,int>(point_look.Key, point_look.Value);
						break;
				}
				bool in_Map = map.Length > looking_at.Key && 0 <= looking_at.Key && map[looking_at.Key].Length > looking_at.Value && 0 <= looking_at.Value;
				// Console.WriteLine("in_Map >> " + in_Map);
				if(in_Map){
					// Console.WriteLine("additional sight");
					// Console.WriteLine("map[looking_at.Key][looking_at.Value] >> " + map[looking_at.Key][looking_at.Value]);
					i_see.Add(looking_at, symbols[map[looking_at.Key][looking_at.Value]]);
					Blocked = !symbols[map[looking_at.Key][looking_at.Value]];
					// Console.WriteLine("look >> " + (sight == look));
					// Console.WriteLine("!Blocked >> " + !Blocked);
					// Console.WriteLine("!memory.Contains(looking_at) >> {0}", !memory.Contains(looking_at));
					// Console.WriteLine("!choices.Contains(looking_at) >> {0}", !choices.Contains(looking_at));
					if(sight == look && !Blocked && !memory.Contains(looking_at) && !choices.Contains(looking_at)){
						choices.Add(looking_at);
					}
					if(Blocked){
						break;
					}
				}else{
					break;
				}
			}
		}
	}

	static Dictionary<Char, Boolean> symbols{
		get{
			Dictionary<Char, Boolean> res = new Dictionary<Char, Boolean>();
			res.Add('b', false);
			res.Add('#', false);
			res.Add(' ', true);
			res.Add('s', true);
			res.Add('f', true);
			res.Add('w', true);
			res.Add('@', true);
			return res;
		}
	}

	static char[][] map;

	static List<KeyValuePair<int,int>> findWay (KeyValuePair<int,int> start, KeyValuePair<int,int> finish) {
		//char[][]
		// locateStartAndFinish(map);
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
		// map[start.Key][start.Value] = 's';
		// map[finish.Key][finish.Value] = 'f';

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
								// temp = new KeyValuePair<int,int>(i, j);
								// path.Add(temp);
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

		// Console.WriteLine("Path Finder >> ");
		// for (int i =0; i < pathFinder.Length; i++){
		// 	Console.Write("|");
		// 	for(int j =0; j < pathFinder[i].Length; j++){
		// 		if(null == pathFinder[i][j]){
		// 			Console.Write(" ");
		// 		}else{
		// 			Console.Write(pathFinder[i][j]);
		// 		}
		// 		Console.Write("|");
		// 	}
		// 	Console.WriteLine();
		// }

		return path;

		// foreach(KeyValuePair<int,int> p in path){
		// 	map[p.Key][p.Value] = '@';
		// }
		// // return map;
		// return null;
	}

	static void update (){

		char[][] current_situation = new char[map.Length][];
		// current_situation.Concat(map);

		for(int i = 0; i < map.Length; i++){
			current_situation[i] = new char[map[i].Length];
			map[i].CopyTo(current_situation[i], 0);
		}

		foreach(Agent a in agents){
			a.doLive();
			Console.WriteLine("Destination >> {0}", a.dist);
			current_situation[a.pos.Key][a.pos.Value] = a.sym;
			// Console.WriteLine("a.i_see.Keys >> {0}", a.i_see.Keys.Count);
			// foreach(KeyValuePair<int,int> coords_see in a.choices){
			// 	current_situation[coords_see.Key][coords_see.Value] = '*';
			// 	// Console.WriteLine("{0} : {1} >> {2}", coords_see.Key, coords_see.Value, current_situation[coords_see.Key][coords_see.Value]);
			// }
		}
		for (int i =0; i < current_situation.Length; i++){
			// Console.Write("|");
			for(int j =0; j < current_situation[i].Length; j++){
				if('w' == current_situation[i][j]){
					Console.Write(' ');
				}else{
					Console.Write(current_situation[i][j]);
				}
				Console.ResetColor();
				// Console.Write(res[i][j]);
				// Console.Write("|");
			}
			Console.WriteLine();
		}
		// Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop-current_situation.Length-1);
	}

	static List<Agent> agents;

	static void Main(string[] args)
	{
		try{
			agents = new List<Agent>();
			Agent Bond = new Agent('@', "Bond");
			Agent Bond2 = new Agent('%', "Bond");
			agents.Add( Bond );
			agents.Add( Bond2 );

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

			int iter = 0;
			while(true){
				update();
				Console.WriteLine("iter >> {0}", iter);
				iter++;
				Console.ReadLine();
			}
		}catch(Exception E){
			Console.WriteLine(E.Message);
			Console.WriteLine(E.StackTrace);
		}

		// foreach(KeyValuePair<char,Boolean> p in symbols){
		// 	Console.WriteLine(p.Key + " >> " + p.Value);
		// }

		// char[][] res = findWay(map);
		// List<KeyValuePair<int,int>> res = findWay(map);
		// Console.WriteLine("result >> ");

		// for(int k = (res.Count-1); k >= 0; k--){
		// 	for (int i =0; i < map.Length; i++){
		// 		// Console.Write("|");
		// 		for(int j =0; j < map[i].Length; j++){
		// 			if(i == res[k].Key && j == res[k].Value){
		// 				Console.Write('@');
		// 			}else if('w' == map[i][j]){
		// 				Console.Write(' ');
		// 			}else{
		// 				Console.Write(map[i][j]);
		// 			}
		// 			// Console.Write(res[i][j]);
		// 			// Console.Write("|");
		// 		}
		// 		Console.WriteLine();
		// 	}
		// 	Console.ReadLine();
		// 	if(0 != k){
		// 		Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop-map.Length-1);
		// 	}
		// }
		return;
	}

}