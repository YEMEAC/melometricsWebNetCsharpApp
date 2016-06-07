#!/usr/bin/env python

import os
import sys
from StringIO import StringIO


 #pathFitFile = "C:/Users/Jeison/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/tests/data/sample-activity.fit"

#redireccionar salida a archivo
#sys.stdout = open("C:/Users/Jeison/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/scripts/updateBitch.txt", "w")

#redireccionar salida
old_stdout = sys.stdout
result = StringIO() 
sys.stdout = result

# Add folder to search path

PROJECT_PATH = os.path.realpath(os.path.join(sys.path[0], '..'))
sys.path.append(PROJECT_PATH)

from fitparse import Activity

quiet = 'quiet' in sys.argv or '-q' in sys.argv
filenames = None

if len(sys.argv) >= 2:
    filenames = [f for f in sys.argv[1:] if os.path.exists(f)]

if not filenames:
    filenames = [os.path.join(PROJECT_PATH, 'tests', 'data', pathFitFile)]




def print_record(rec, ):
    global count
    global record_number
    record_number += 1
    count = 0
	
    #print ("-- %d. #%d: %s (%d entries) " % (record_number, rec.num, rec.type.name, len(rec.fields))).ljust(60, '-')
    to_print=""
    for field in rec.fields:
		if len(rec.fields) >= 6 and (field.name == 'timestamp' or field.name == 'position_lat' or field.name == 'position_long'  or field.name == 'distance' or field.name =='speed' or  field.name =='heart_rate'):
			to_print += "%s|%s|" % (field.name, field.data)
			count=count+1
			#if field.data is not None and field.units:
				#to_print += " [%s]" % field.units
				
    if count==6:
	     #print to_print
        sys.stdout.write(to_print)

for f in filenames:
    #if quiet:
        #print f
    #else:
        #print ('##### %s ' % f).ljust(60, '#')

    print_hook_func = None
    if not quiet:
        print_hook_func = print_record

    record_number = 0
    a = Activity(f)
    a.parse(hook_func=print_hook_func)

#redireccionar salida
sys.stdout = old_stdout
result_string = result.getvalue()

