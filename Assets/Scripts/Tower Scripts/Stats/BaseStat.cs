// Base Class for all stats in the game.

public class BaseStat {
	// Stat Properties
	private int _baseValue;			// Base value of this stat.


	public BaseStat (int val) {
		_baseValue = val;
	}

	public BaseStat () {
		_baseValue = 0;
	}

	public int BaseValue {
		get{ return _baseValue; }
		set{ _baseValue = value; }
	}
}
