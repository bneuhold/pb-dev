/*global $ */

var slider = (function() {
	'use strict';

	var $slider = $('#slider');
	var $sliderInner = $('#sliderInner', $slider);
	var $controls = $('#controls', $slider);
	var slideNumber = $( 'article', $slider).size();
	var slideWidth = 800 ;
	var totalWidth = slideNumber * slideWidth ;
	var current = 0;
	var timer;

	$sliderInner.css('width',totalWidth);

	function slide () {
		$sliderInner.animate({ left: -(current*slideWidth) }, 500);
		$( 'a', $controls).removeClass('active').eq(current).addClass('active');
	}

	function goLeft () {
		current++;
		if (current >= slideNumber) {
			current = 0;
		}
		slide();
	}

	function goRight () {
		current--;
		if (current < 0) {
			current = slideNumber-1;
		}
		slide();
	}

	function goto(number) {
		current = number;
		slide();
	}

	function bindClicks () {
		$('a', $controls).each(function (i,elem) {
			$(elem).data('number',i);
		}).click(function (e) {
			e.preventDefault();
			var number = $(this).data('number');
			goto(number);
		});
	}

	function bindHover () {
		$slider.hover(function () {
			stopTimer();
		}, function () {
			startTimer();
		});
	}

	function execTimer () {
		goLeft();
		timer = setTimeout(execTimer, 5000);
	}

	function startTimer () {
		timer = setTimeout(execTimer, 5000);
	}

	function stopTimer() {
		clearTimeout(timer);
	}

	function init () {
		bindClicks();
		bindHover();
		startTimer();
	}

	init();

	return {
		goto: goto
	};

})();

function Timer(time, object) {
	'use strict';

	var now;
	var self = this;

	this.time = time ;
	this.object = object;
	this.$timer = object;
	this.$days = $('.days', object);
	this.$hours = $('.hours', object);
	this.$minutes = $('.minutes', object);
	this.$seconds = $('.seconds', object);

	this.days = 0;
	this.hours = 0;
	this.minutes = 0;
	this.seconds = 0;

	function execTimer () {

		self.$days.text(self.days);
		self.$hours.text(self.hours);
		self.$minutes.text(self.minutes);
		self.$seconds.text(self.seconds);

		self.seconds-=1;

		if (self.seconds < 0) {
			self.seconds = 59;
			self.minutes-=1;
		}

		if (self.minutes < 0) {
			self.minutes = 59;
			self.hours-=1;
		}

		if (self.hours < 0) {
			self.hours = 23;
			self.days-=1;
		}

		if (self.days < 0) {
			self.$timer.hide();
		}

		setTimeout(execTimer, 1000);
	}

	this.init = function () {
		now = new Date();
		var diff = self.time-now;

		self.days = Math.floor(diff/1000/60/60/24);

		diff -= self.days*1000*60*60*24;

		self.hours = Math.floor(diff/1000/60/60);
		diff -= self.hours*1000*60*60;

		self.minutes = Math.floor(diff/1000/60);
		diff -= self.minutes*1000*60;

		self.seconds = Math.floor(diff/1000);

		execTimer();

		return self;
	};

	return this.init();
};

var timers = (function(){
	'use strict';

	var timers = [];

	var size = function () {
		return timers.length;
	};



	var addTimer = function (object) {
		var id = size();
		var $timer = $('.timer', object);
		var time;

		try {
			time = new Date( $timer.data('time') );
		} catch (err)  {
			return false;
		}

		timers.push( new Timer(time, $timer) );

		return id;
	};

	var addAll = function () {
		$('.time-left').each(function  (i,elem) {
			addTimer(elem);
		});
	};

	return {
		size: size,
		addAll: addAll
	};
})();

function citySelector() {
	'use strict';

	$('.city-selector ul').hover(function () {
			$(this).css('height', 'auto');
		}, function () {
			$(this).css('height', '33px');
		}
	);
}


function init () {
	'use strict';

	timers.addAll();
	citySelector();
}

$(init);