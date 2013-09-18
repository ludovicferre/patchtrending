using System;
using System.Collections.Generic;
using System.Text;

namespace Symantec.CWoC.PatchTrending {
    class StaticStrings {
        #region // Landing (html, javascript)
        public static string html_Landing = @"<html>
	<head>
        <title>{CWoC} Patch Trending home</title>
        <style type=""text/css"">
        ul { width: 60em; }
        ul li {  float: left; width: 20em;  }
        br { clear: left; }
        div.wrapper {  margin-bottom: 1em;}
		body { font-family: Arial;};
        </style>
	</head>
    <body style=""width:1000"">
    <h2 style=""text-align: center; width:80%"">Global Compliance view</h2>
    <hr/>
    <table style=""width: 80%"">
        <tr>
            <td style=""text-align: center""><b>Installed versus Applicable</b></td> 
            <td style=""text-align: center""><b>Compliance status in %</b></td>
        </tr>
        <tr>
            <td><div id='global_div_1' style='width: 500px; height: 300px;'></div></td>
            <td><div id='global_div_0' style='width: 500px; height: 300px;'></div></td>
        </tr>
    </table>";

        public static string LandingJs = @"
	<script type=""text/javascript"" src=""https://www.google.com/jsapi""></script>
    <script src=""https://ajax.googleapis.com/ajax/libs/prototype/1.7.0.0/prototype.js""></script>
	<script type=""text/javascript"" src=""javascript/global_0.js""></script>
	<script type=""text/javascript"" src=""javascript/global_1.js""></script>
    <script type=""text/javascript"" src=""javascript/pccompl.js""></script>
    <script type=""text/javascript"" src=""javascript/inactive_computers.js""></script>
    <script type=""text/javascript"" src=""javascript/inactive_computers_pc.js""></script>
	<script type=""text/javascript"" src=""javascript/global.js""></script>
	<script type=""text/javascript"">
		google.load(""visualization"", ""1"", {packages:[""corechart""]});
		google.setOnLoadCallback(drawChart);

        function loadBulletin() {

			var bulletin = document.getElementById(""bulletin_name"").value;
			var jsUrl = ""javascript/"" + escapeBulletin(bulletin) + ""_0.js"";

			new Ajax.Request(jsUrl, {
                method:'get',
				onSuccess: function(response) {
					// Handle the response content...
					if (response.responseText.length > 0) {
						results = eval(response.responseText);
						window.location = ""getbulletin.html?"" + bulletin;
					}
				}, 
				onFailure: function() {
					alert(""Could not find any data to load for bulletin "" + document.getElementById(""bulletin_name"").value);
				}

			});	
		}

		function escapeBulletin(b) {
			var t = b.replace(""-"", ""_"");
            t = t.replace(""."", ""_"");
			return t.toUpperCase();
        }
    </script>
    <script type=""text/javascript"">

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "","");
        }

		var msg_a = analyse_compliance();
		var msg_b = analyse_vulnerability();
		var msg_c = analyse_pccompl();

		var box = document.getElementById(""daily_summary"");
		box.innerHTML += ""<hr/><h3>Daily summary</h3>"";
		box.innerHTML += ""<p>"" + msg_a + ""</p>"";
		box.innerHTML += ""<p>"" + msg_b + ""</p>"";
		box.innerHTML += ""<p>"" + msg_c + ""</p>"";

		function analyse_compliance () {
			var length = GLOBAL_0.length;
			var low_point = ['', 100];
			var high_point = ['', 0];
			var first = GLOBAL_0[1];
			var last = GLOBAL_0[length -1];

			for (var i = 1; i < length; i++) {
				current = GLOBAL_0[i];
				prev = GLOBAL_0[i - 1];

				// Track high and low point
				if (current[1] > high_point[1]) {
					high_point = current;
				}
				if (current[1] < low_point[1]) {
					low_point = current;
				}
			}
			var delta = last[1] - first[1];
			var append = """";
			if (delta > 0) {
				append = ""+"";
			}
			var hist_high = """";
			var hist_low = """";

			if (high_point[1] == last[1])
				hist_high = ""We are at a historical high from the "" + (length -1) + "" records available. "";
			else
				hist_high = ""The historical high value of "" + Math.round(high_point[1]*100)/100 + ""% was recorded on "" + high_point[0] + "". "";

			if 	(low_point[1] == last[1])
				hist_low = ""We are at a historical low from the "" + (length -1) + "" records available. "";
			else
				hist_low = ""The historical low value of "" + Math.round(low_point[1]*100)/100 + ""% was recorded on "" + low_point[0] + "". "";

			var msg = ""Compliance is at <b>"" + Math.round(last[1]*100)/100 
					+ ""%</b> ("" + last[0] + ""), ""
					+ ""from "" + Math.round(first[1]*100)/100 
					+ ""% ("" + append + """" + Math.round(delta*100)/100
					+ ""%) on the "" + first[0] + "". "" + hist_high + hist_low
			;

			return msg;
		}

		function analyse_vulnerability () {
			var length = GLOBAL_1.length;
			var low_point = ['', 0, 0, 999999999];
			var high_point = ['', 0, 0, 0];
			var first = GLOBAL_1[1];
			var last = GLOBAL_1[length -1];

			for (var i = 1; i < length; i++) {
				current = GLOBAL_1[i];
				prev = GLOBAL_1[i - 1];

				// Track high and low point
				if (current[3] > high_point[3]) {
					high_point = current;
				}
				if (current[3] < low_point[3]) {
					low_point = current;
				}
			}

			var delta = last[3] - first[3];
			var append = """";
			if (delta > 0) {
				append = ""+"";
			}

			var hist_high = """";
			var hist_low = """";

			if (high_point[1] == last[1])
				hist_high = ""We are at a historical high from the "" + (length -1) + "" records available. "";
			else
				hist_high = ""The historical high value of "" + numberWithCommas(high_point[3]) + "" vulnerable updates was recorded on "" + high_point[0] + "". "";

			if 	(low_point[1] == last[1])
				hist_low = ""We are at a historical low from the "" + (length -1) + "" records available. "";
			else
				hist_low = ""The historical low value of "" + numberWithCommas(low_point[3]) + "" vulnerable updates was recorded on "" + low_point[0] + "". "";

			var msg = ""We currently have <b>"" + numberWithCommas(last[3])
					+ "" vulnerable updates</b> ("" + last[0] + ""), ""
					+ ""from "" + numberWithCommas(first[3])
					+ "" vulnerable updates ("" + append + """" + numberWithCommas(delta)
					+ "") on the "" + first[0] + "". "" + hist_high + hist_low
			;

			return msg;
		}

		function analyse_pccompl () {
			var length = pccompl.length;
            var compl_all = 0;
			var compl_top = 0;
			var compl_mid = 0;

			if (length == 0)
				return '';

			for (var i = 1; i < length; i++) {
				if (parseInt(pccompl[i]) > 74) {
					var s = pccompl[i][5];
					var j = s.indexOf('(') + 1;
					var k = s.indexOf('% of');

					compl_all += parseFloat(s.substring(j, k))
				}
				if (parseInt(pccompl[i]) > 89) {
					var s = pccompl[i][5];
					var j = s.indexOf('(') + 1;
					var k = s.indexOf('% of');

					compl_mid += parseFloat(s.substring(j, k))
				}
				if (parseInt(pccompl[i]) > 94) {
					var s = pccompl[i][5];
					var j = s.indexOf('(') + 1;
					var k = s.indexOf('% of');

					compl_top += parseFloat(s.substring(j, k))
				}
			}

			var msg = '~<b>' + compl_top.toFixed(2) + '%</b> of computers are at <i>95%</i> compliance or above';
			msg += ', ~<b>' + compl_mid.toFixed(2) + '%</b> of computers are at <i>90%</i> compliance or above';
			msg += ' and ~<b>' + compl_all.toFixed(2) + '%</b> of computers are at <i>75%</i> compliance or above</i>.';

			return msg;
		}
		</script>
";

        public static string html_PcCompl_div = @"
    <hr/>
    <p style=""text-align: center;""><b>Compliance by computer - upper quarter</b></p>
    <div id='pccompl_div' style='width: 1000px; height: 300px;'></div>";

        public static string html_PcComplAndInactive_div = @"
    <hr/>
    <table style=""width: 80%"">
        <tr>
            <td style=""text-align: center""><b>Compliance by Computer (85%+)</b></td> 
            <td style=""text-align: center""><b>Inactive computers (in % of managed)</b></td>
        </tr>
        <tr>
            <td><div id='pccompl_div' style='width: 500px; height: 300px;'></div></td>
            <td><div id='inactivepc_div' style='width: 500px; height: 300px;'></div></td>
        </tr>
    </table>
";

        public static string html_PcInactive_div = @"
    <hr/>
    <p style=""text-align: center;""><b>Inactive computers (in % of managed)</b></p>
    <div id='inactivepc_div' style='width: 1000px; height: 300px;'></div>";

        public static string html_DailySummary_div = @"
	<div id=""daily_summary""></div>
";

        public static string html_BulletinSearch = @"
    <hr/>
	Bulletin name: <input type=""text"" id=""bulletin_name""></input><input type=""button"" value=""View graphs"" onclick=""loadBulletin()""/>
    <hr/>";

        #endregion

        #region // string js_GlobalCompliance
        public static string js_GlobalCompliance = @"
        function drawChart() {
	        var options1 = { title: '', vAxis: { maxValue : 100, minValue : 0 }};
	        var options2 = { title: '', vAxis: { minValue : 0 }};

		    var d_global_0 = google.visualization.arrayToDataTable(GLOBAL_0);
		    var g_global_0 = new google.visualization.LineChart(document.getElementById('global_div_0'));
		    g_global_0.draw(d_global_0, options1);

		    var d_global_1 = google.visualization.arrayToDataTable(GLOBAL_1);
		    var g_global_1 = new google.visualization.LineChart(document.getElementById('global_div_1'));
		    g_global_1.draw(d_global_1, options2);

            if (pccompl.length > 0) {
		        var d_pccompl = new google.visualization.DataTable();
                d_pccompl.addColumn('number', 'Compliance in %');
		        d_pccompl.addColumn('number');
		        d_pccompl.addColumn('number');
		        d_pccompl.addColumn('number');
		        d_pccompl.addColumn('number');
		        d_pccompl.addColumn({type:'string', role:'tooltip', 'p': {'html': true}});
		        d_pccompl.addRows(pccompl);

                var g_pccompl = new google.visualization.CandlestickChart(document.getElementById('pccompl_div'));
                g_pccompl.draw(d_pccompl, { legend:'none', tooltip: {isHtml: true}} );
            }

            if (inactive_computers_pc.length > 0) {
                var d_inactive = google.visualization.arrayToDataTable(inactive_computers_pc);
                var g_inactive = new google.visualization.LineChart(document.getElementById('inactivepc_div'));
                g_inactive.draw(d_inactive, {colors: ['orange', 'red', 'royalblue', 'forestgreen']});	
            }
        }";
        #endregion

        #region // SQL query strings
        public static string sql_get_bulletins_in = @"
               -- Get all tracked bulletins
                select bulletin
                  from TREND_WindowsCompliance_ByUpdate
                 where bulletin in ({0})
                 group by bulletin
                 order by MIN(_exec_time) desc, Bulletin desc";
        public static string sql_get_all_bulletins = @"
               -- Get all tracked bulletins
                select bulletin
                  from TREND_WindowsCompliance_ByUpdate
                 group by bulletin
                 order by MIN(_exec_time) desc, Bulletin desc";
        public static string sql_get_global_compliance_data = @"
                         select Convert(Datetime, max(_Exec_time), 101) as 'Date', SUM(installed) as 'Installed', SUM(Applicable) as 'Applicable'
                           from TREND_WindowsCompliance_ByUpdate
                          group by _Exec_id order by date";
        public static string sql_get_top10_vulnerable = @"
                select top 10 Bulletin --, SUM(Applicable) - SUM(installed)
                  from TREND_WindowsCompliance_ByUpdate
                 where _Exec_id = (select MAX(_exec_id) from TREND_WindowsCompliance_ByUpdate)
                 group by Bulletin
                 order by SUM(Applicable) - SUM(installed) desc";
        public static string sql_get_bottom10_compliance = @"
                select top 10 Bulletin --, CAST(SUM(installed) as float) / CAST(SUM(Applicable) as float) * 100
                  from TREND_WindowsCompliance_ByUpdate
                 where _Exec_id = (select MAX(_exec_id) from TREND_WindowsCompliance_ByUpdate)
                 group by Bulletin
                having SUM(Applicable) - SUM(installed) > 100
                 order by CAST(SUM(installed) as float) / CAST(SUM(Applicable) as float) * 100";
        public static string sql_get_top10movers_up = @"
                -- Return the 10 bulletins for which more computers are secured
                select top 10 t1.Bulletin, t1._Exec_id, (sum(t2.Applicable) - SUM(t2.installed)) - (sum(t1.Applicable) - SUM(t1.installed)) as 'Delta'
                  from TREND_WindowsCompliance_ByUpdate t1
                  join TREND_WindowsCompliance_ByUpdate t2
                    on t1._Exec_id -1 = t2._Exec_id and t1.Bulletin = t2.Bulletin and t1.[UPDATE] = t2.[update]
                 where t1._Exec_id = (select MAX(_exec_id) from TREND_WindowsCompliance_ByUpdate)
                 group by t1.Bulletin, t1._Exec_id
                having (sum(t2.Applicable) - SUM(t2.installed)) - (sum(t1.Applicable) - SUM(t1.installed)) > 0
                 order by (sum(t2.Applicable) - SUM(t2.installed)) - (sum(t1.Applicable) - SUM(t1.installed)) desc
                ";
        public static string sql_get_top10movers_down = @"
                -- Return the 10 bulletins for which more computers are vulnerable
                select top 10 t1.Bulletin, t1._Exec_id, (sum(t2.Applicable) - SUM(t2.installed)) - (sum(t1.Applicable) - SUM(t1.installed)) as 'Delta'
                  from TREND_WindowsCompliance_ByUpdate t1
                  join TREND_WindowsCompliance_ByUpdate t2
                    on t1._Exec_id -1 = t2._Exec_id and t1.Bulletin = t2.Bulletin and t1.[UPDATE] = t2.[update]
                 where t1._Exec_id = (select MAX(_exec_id) from TREND_WindowsCompliance_ByUpdate)
                 group by t1.Bulletin, t1._Exec_id
                having (sum(t2.Applicable) - SUM(t2.installed)) - (sum(t1.Applicable) - SUM(t1.installed)) < 0
                 order by (sum(t2.Applicable) - SUM(t2.installed)) - (sum(t1.Applicable) - SUM(t1.installed))
                ";
        public static string sql_get_updates_bybulletin = @"
                 select distinct([UPDATE])
                   from TREND_WindowsCompliance_ByUpdate
                  where bulletin = '{0}'
                 ";

        public static string sql_get_compliance_bypccount = @"
declare @id as int
	set @id = (select MAX(_exec_id) from TREND_WindowsCompliance_ByComputer)

if (@id > 1)
begin
	select t1.[Percent], t3.[min], t2.[Computer #], t1.[Computer #], t3.[max], t2.[% of Total]

--	, t1.[% of Total], t2.[% of Total]
	  from TREND_WindowsCompliance_ByComputer t1
	  join TREND_WindowsCompliance_ByComputer t2
		on t1.[Percent] = t2.[Percent]
	  join (
				select[Percent], MIN(t3.[Computer #]) as min, MAX(t3.[computer #]) as max
				  from TREND_WindowsCompliance_ByComputer t3
				group by [Percent]
			) t3
	    on t1.[Percent] = t3.[percent]	    
	 where t1._Exec_id = @id
	   and t2._Exec_id = @id - 1
--	   and t1.[Percent] > 74
end
";

        public static string sql_get_compliance_bypcpercent = @"declare @id as int
	set @id = (select MAX(_exec_id) from TREND_WindowsCompliance_ByComputer)

if (@id > 1)
begin
	select t1.[Percent], t3.[min], t2.[% of Total], t1.[% of Total], t3.[max], t1.[% of Total]
--	, t1.[% of Total], t2.[% of Total]
	  from TREND_WindowsCompliance_ByComputer t1
	  join TREND_WindowsCompliance_ByComputer t2
		on t1.[Percent] = t2.[Percent]
	  join (
				select[Percent], MIN(t3.[% of Total]) as min, MAX(t3.[% of Total]) as max
				  from TREND_WindowsCompliance_ByComputer t3
				group by [Percent]
			) t3
	    on t1.[Percent] = t3.[percent]	    
	 where t1._Exec_id = @id
	   and t2._Exec_id = @id - 1
--	   and t1.[Percent] > 74
end
";
        public static string sql_get_compliance_bypc_bottom75percent = @"
/* BOTTOM 75% SUMMARY */
declare @id as int
	set @id = (select MAX(_exec_id) from TREND_WindowsCompliance_ByComputer)

select SUM([computer #]), SUM([% of total])
  from TREND_WindowsCompliance_ByComputer t3
 where t3._Exec_id = @id
   and t3.[Percent] < 75
 group by [_exec_id]
";
        public static string sql_get_inactive_computer_trend = @"
select timestamp, [Inactive computers (7 days)], [Inactive computers (17 days)], [New inactive computers], [New Active Computers]
  from TREND_InactiveComputerCounts
 order by _exec_id
";
        public static string sql_get_inactive_computer_percent = @"
select timestamp, cast([Inactive computers (7 days)] as money) /  cast([Managed machines] as money) * 100 as '7-days inactive (% of managed)', cast([Inactive computers (17 days)] as money) /  cast([Managed machines] as money) * 100 as '17-days inactive (% of managed)', CAST([New inactive computers] as money) / CAST([Managed machines] AS money) * 100 as '++ (% of managed)', CAST([New active computers] as money) / CAST([Managed machines] as money) * 100 as '-- (% of managed)'
  from TREND_InactiveComputerCounts
 order by _exec_id
     ";

#endregion

        #region // string html_GetBulletin_page
        public static string html_GetBulletin_page = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
	<title>Bulletin detailed view</title>
	<script type='text/javascript' src='http://www.google.com/jsapi'></script>
	<script type='text/javascript' src='javascript/pccompl_full.js'></script>
	<script type='text/javascript'>google.load('visualization', '1.1', {packages: ['corechart', 'controls']});</script>
</head>
<body>
	<h3>Installed versus Applicable</h3>
    <div id='dashboard_vuln'>
        <div id='chart_vuln' style='width: 915px; height: 300px;'></div>
        <div id='control_vuln' style='width: 915px; height: 50px;'></div>
    </div>
	<h3>Compliance in %</h3>
    <div id='dashboard_compl'>
        <div id='chart_compl' style='width: 915px; height: 300px;'></div>
        <div id='control_compl' style='width: 915px; height: 50px;'></div>
    </div>
  </body>
	<script type='text/javascript'>
		var bulletin = window.location.search.substring(1).toUpperCase();

		function loadjs(filename){
			var fileref = document.createElement('script')
			fileref.setAttribute('type','text/javascript')
			fileref.setAttribute('src', filename)
			
			if (typeof fileref!='undefined')
				document.getElementsByTagName('head')[0].appendChild(fileref)
		}
		
		function escapeBulletin(b) {
			var t = b.replace('-', '_');
			return t.replace('.', '_');
		}
		 
		loadjs('javascript/' + escapeBulletin(bulletin) + '_0.js');
		loadjs('javascript/' + escapeBulletin(bulletin) + '_1.js');

		function drawVisualization() {
		
			b = escapeBulletin(bulletin);			
			var data_compl = window[b + '_0'];
			for (var i = 1; i < data_compl.length; i++) {
				data_compl[i][0] = data_compl[i][0].split(' ');
				data_compl[i][0] = data_compl[i][0][0].split('/');
				data_compl[i][0] = data_compl[i][0].reverse().join('-');
				data_compl[i][0] = new Date(Date.parse(data_compl[i][0]));
			}

			var dashboard_compl = new google.visualization.Dashboard(document.getElementById('dashboard_compl'));
			var dashboard_vuln = new google.visualization.Dashboard(document.getElementById('dashboard_vuln'));

			var control_compl = new google.visualization.ControlWrapper({
					'controlType': 'ChartRangeFilter',
					'containerId': 'control_compl',
					'options': {
					'filterColumnIndex': 0,
					'ui': {
						'chartType': 'LineChart',
						'chartOptions': {
							'chartArea': {'width': '90%'},
							'hAxis': {'baselineColor': 'none'}
						},
						'chartView': {
							'columns': [0, 1]
						}, 'minRangeSize': 7
						}
					}
				});

			var chart_compl = new google.visualization.ChartWrapper({
				'chartType': 'LineChart',
				'containerId': 'chart_compl',
				'options': {
				'chartArea': {'height': '80%', 'width': '90%'},
				'tooltip': {isHtml: false},
				'hAxis': {'slantedText': false},
				'vAxis': {'viewWindow': {'min': 0, 'max': 101}},
				'legend': {'position': 'none'}
				},
			});

			var d_compl = new google.visualization.arrayToDataTable(data_compl);

			dashboard_compl.bind(control_compl, chart_compl);
			dashboard_compl.draw(d_compl);

						var data_vuln = window[b + '_1'];
			for (var i = 1; i < data_vuln.length; i++) {
				data_vuln[i][0] = data_vuln[i][0].split(' ');
				data_vuln[i][0] = data_vuln[i][0][0].split('/');
				data_vuln[i][0] = data_vuln[i][0].reverse().join('-');
				data_vuln[i][0] = new Date(Date.parse(data_vuln[i][0]));
			}

			var dashboard_vuln = new google.visualization.Dashboard(document.getElementById('dashboard_vuln'));
			var dashboard_vuln = new google.visualization.Dashboard(document.getElementById('dashboard_vuln'));

			var control_vuln = new google.visualization.ControlWrapper({
					'controlType': 'ChartRangeFilter',
					'containerId': 'control_vuln',
					'options': {
					'filterColumnIndex': 0,
					'ui': {
						'chartType': 'LineChart',
						'chartOptions': {
							'chartArea': {'width': '90%'},
							'hAxis': {'baselineColor': 'none'}
						},
						'chartView': {
							'columns': [0, 1]
						}, 'minRangeSize': 7
						}
					}
				});

			var chart_vuln = new google.visualization.ChartWrapper({
				'chartType': 'LineChart',
				'containerId': 'chart_vuln',
				'options': {
				'chartArea': {'height': '80%', 'width': '90%'},
				'tooltip': {isHtml: false},
				'hAxis': {'slantedText': false},
				'vAxis': {'viewWindow': {'min': 0}},
				'legend': {'position': 'none'}
				},
			});

			var d_vuln = new google.visualization.arrayToDataTable(data_vuln);

			dashboard_vuln.bind(control_vuln, chart_vuln);
			dashboard_vuln.draw(d_vuln);

			}

		google.setOnLoadCallback(drawVisualization);
	</script>
</html>
";
        #endregion

        #region //string html_GetInactiveComputers_page
        public static string html_GetInactiveComputers_page = @"<html>
  <head>
    <title>Inactive computer trending</title>
    <script type=""text/javascript"" src=""https://www.google.com/jsapi""></script>
	<script type=""text/javascript"" src=""javascript/inactive_computers.js""></script>
	<script type=""text/javascript"" src=""javascript/inactive_computers_pc.js""></script>
    <script type=""text/javascript"">
      google.load(""visualization"", ""1"", {packages:[""corechart""]});
      google.setOnLoadCallback(switch_view);
	  
	  var data;
	  var options;

      function drawChart() {
        
        var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
        chart.draw(data, options);	
      }
	  
	  var percent = false;
	  function switch_view() {
		if (percent) {
			percent = false;
			data = google.visualization.arrayToDataTable(inactive_computers_pc);
	        options = {
			  title: '(in % of managed machines)', 
			  colors: ['orange', 'red', 'royalblue', 'forestgreen']
			};
			drawChart();
		} else {
			percent = true;
			data = google.visualization.arrayToDataTable(inactive_computers);
	        options = {
			  title: '(in computer count)',
			  colors: ['orange', 'red', 'royalblue', 'forestgreen']
			};
			drawChart();
		}
	  }
    </script>
  </head>
  <body>
	<h2>Inactive computers over time</h2>
    <div id=""chart_div"" style=""width: 900px; height: 500px;""></div>
	<a href=""javascript:switch_view()"" title=""Click to switch between Computer Count view and % of Managed machines view."">Switch View</a>
	<p><b><i>Note:</i></b> ""7 days ++"" reports the number of new inactive computers from the previous snapshot. ""7 days --"" reports the count of computers that were inactive in the previous snapshos and are not in the current snapshot (they are back to Active!).</p>
  </body>
</html>
";
        #endregion

        #region string html_ComputerCompliance_page
        public static string html_ComputerCompliance_page = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
	<title>Compliance by Computer</title>
	<script type='text/javascript' src='http://www.google.com/jsapi'></script>
	<script type='text/javascript' src='javascript/pccompl_full.js'></script>
	<script type='text/javascript'>google.load('visualization', '1.1', {packages: ['corechart', 'controls']});</script>
</head>
<body>
	<h2>Compliance by Computer</h2> 
    <div id='dashboard'>
        <div id='chart' style='width: 915px; height: 400px;'></div>
        <div id='control' style='width: 915px; height: 50px;'></div>
    </div>
  </body>
	<script type='text/javascript'>
		function drawVisualization() {
			var dashboard = new google.visualization.Dashboard(document.getElementById('dashboard'));

			var control = new google.visualization.ControlWrapper({
					'controlType': 'ChartRangeFilter',
					'containerId': 'control',
					'options': {
					'filterColumnIndex': 0,
					'ui': {
						'chartType': 'LineChart',
						'chartOptions': {
							'chartArea': {'width': '90%'},
							'hAxis': {'baselineColor': 'none'}
						},
						'chartView': {
							'columns': [0, 3]
						},
						'minRangeSize': 10
						}
					},
					'state': {'range': {'start': 80, 'end': 100}}
				});

			var chart = new google.visualization.ChartWrapper({
				'chartType': 'CandlestickChart',
				'containerId': 'chart',
				'options': {
				'chartArea': {'height': '80%', 'width': '90%'},
				'tooltip': {isHtml: true},
				'hAxis': {'slantedText': false},
				'vAxis': {'viewWindow': {'min': 0}},
				'legend': {'position': 'none'}
				},
			});

			var d_pccompl = new google.visualization.DataTable();
			d_pccompl.addColumn('number', 'Compliance in %');
			d_pccompl.addColumn('number');
			d_pccompl.addColumn('number');
			d_pccompl.addColumn('number');
			d_pccompl.addColumn('number');
			d_pccompl.addColumn({type:'string', role:'tooltip', 'p': {'html': true}});
			d_pccompl.addRows(pccompl_full);

			dashboard.bind(control, chart);
			dashboard.draw(d_pccompl);
		}

		google.setOnLoadCallback(drawVisualization);
	</script>
</html>
";
        #endregion
    }
}
