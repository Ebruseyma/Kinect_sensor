import csv
from flask import (Flask, redirect,  # importing Flask framework
                   render_template, request, url_for)
from flask.wrappers import Response


app = Flask(__name__)

import pandas as pd
from sklearn.model_selection import train_test_split

data = pd.read_csv('C:\\Users\\user\\Desktop\\gaitApp\\1\\Dataset(1).csv', sep=';')  #read data from csv
data = pd.DataFrame(data)

data["speedmin"] = data["speedmin"].str.replace(",", ".")     #change ',' to '.'
data["speedavg"] = data["speedavg"].str.replace(",", ".")
data["speedmax"] = data["speedmax"].str.replace(",", ".")
data["steplengthmin"] = data["steplengthmin"].str.replace(",", ".")
data["steplengthmax"] = data["steplengthmax"].str.replace(",", ".")
data["stridelengthmin"] = data["stridelengthmin"].str.replace(",", ".")
data["stridelengthmax"] = data["stridelengthmax"].str.replace(",", ".")
data["steptimemin"] = data["steptimemin"].str.replace(",", ".")
data["steptimeavg"] = data["steptimeavg"].str.replace(",", ".")
data["steptimemax"] = data["steptimemax"].str.replace(",", ".")
data["stridetimemin"] = data["stridetimemin"].str.replace(",", ".")
data["stridetimeavg"] = data["stridetimeavg"].str.replace(",", ".")
data["stridetimemax"] = data["stridetimemax"].str.replace(",", ".")
data["onemeterdistance"] = data["onemeterdistance"].str.replace(",", ".")

data["speedmin"] = data.speedmin.astype(float)               #change data type object to float
data["speedavg"] = data.speedavg.astype(float)
data["speedmax"] = data.speedmax.astype(float)
data["steplengthmin"] = data.steplengthmin.astype(float)
data["steplengthmax"] = data.steplengthmax.astype(float)
data["stridelengthmin"] = data.stridelengthmin.astype(float)
data["stridelengthmax"] = data.stridelengthmax.astype(float)
data["steptimemin"] = data.steptimemin.astype(float)
data["steptimeavg"] = data.steptimeavg.astype(float)
data["steptimemax"] = data.steptimemax.astype(float)
data["stridetimemin"] = data.stridetimemin.astype(float)
data["stridetimeavg"] = data.stridetimeavg.astype(float)
data["stridetimemax"] = data.stridetimemax.astype(float)
data["onemeterdistance"] = data.onemeterdistance.astype(float)

X = data.iloc[:, 0:54]      #for train
y = data.iloc[:, 54:55]     #for test

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.20,random_state=42)

from sklearn.ensemble import RandomForestClassifier

model = RandomForestClassifier(random_state=42)   #random forest modelling
model.fit(X_train, y_train.values.ravel())        #model fitting

@app.route('/',methods=['GET', 'POST']) 
def index():                                  #sends data to index.html
    request_data = request.get_json()         #get json data from kinect
    if request_data != None:
     totalseconds = request_data.get("totalseconds")
     stepcount = request_data.get("stepcount")
     stridecount = request_data.get("stridecount")
     totaldistance = request_data.get("totaldistance")
     speedmin = request_data.get("speedmin")
     speedavg = request_data.get("speedavg")
     speedmax = request_data.get("speedmax")
     steplengthmin = request_data.get("steplengthmin")
     steplengthavg = request_data.get("steplengthavg")
     steplengthmax = request_data.get("steplengthmax")
     stridelengthmin = request_data.get("stridelengthmin")
     stridelengthavg = request_data.get("stridelengthavg")
     stridelengthmax = request_data.get("stridelengthmax")
     cadence = request_data.get("cadence")
     steptimemin = request_data.get("steptimemin")
     steptimeavg = request_data.get("steptimeavg")
     steptimemax = request_data.get("steptimemax")
     stridetimemin = request_data.get("stridetimemin")
     stridetimeavg = request_data.get("stridetimeavg")
     stridetimemax = request_data.get("stridetimemax")
     footwidthmin = request_data.get("footwidthmin")
     footwidthavg = request_data.get("footwidthavg")
     footwidthmax = request_data.get("footwidthmax")
     kneewidthmin = request_data.get("kneewidthmin")
     kneewidthavg = request_data.get("kneewidthavg")
     kneewidthmax = request_data.get("kneewidthmax")
     onemeterdistance = request_data.get("onemeterdistance")
     rightkneeanglemin = request_data.get("rightkneeanglemin")
     rightkneeangleavg = request_data.get("rightkneeangleavg")
     rightkneeanglemax = request_data.get("rightkneeanglemax")
     rightankleanglemin = request_data.get("rightankleanglemin")
     rightankleangleavg = request_data.get("rightankleangleavg")
     rightankleanglemax = request_data.get("rightankleanglemax")
     righthipanglemin = request_data.get("righthipanglemin")
     righthipangleavg = request_data.get("righthipangleavg")
     righthipanglemax = request_data.get("righthipanglemax")
     leftkneeanglemin = request_data.get("leftkneeanglemin")
     leftkneeangleavg = request_data.get("leftkneeangleavg")
     leftkneeanglemax = request_data.get("leftkneeanglemax")
     leftankleanglemin = request_data.get("leftankleanglemin")
     leftankleangleavg = request_data.get("leftankleangleavg")
     leftankleanglemax = request_data.get("leftankleanglemax")
     lefthipanglemin = request_data.get("lefthipanglemin")
     lefthipangleavg = request_data.get("lefthipangleavg")
     lefthipanglemax = request_data.get("lefthipanglemax")
     rightarmanglemin = request_data.get("rightarmanglemin")
     rightarmangleavg = request_data.get("rightarmangleavg")
     rightarmanglemax = request_data.get("rightarmanglemax")
     leftarmanglemin = request_data.get("leftarmanglemin")
     leftarmangleavg = request_data.get("leftarmangleavg")
     leftarmanglemax = request_data.get("leftarmanglemax")
     backanglemin = request_data.get("backanglemin")
     backangleavg = request_data.get("backangleavg")
     backanglemax = request_data.get("backanglemax")

     predict = model.predict([[totalseconds, stepcount, stridecount, totaldistance, speedmin, speedavg,                   #predict using model
                                            speedmax, steplengthmin, steplengthavg, steplengthmax,
                                            stridelengthmin, stridelengthavg, stridelengthmax, cadence, steptimemin,
                                            steptimeavg, steptimemax, stridetimemin, stridetimeavg,
                                            stridetimemax, footwidthmin, footwidthavg, footwidthmax, kneewidthmin,
                                            kneewidthavg, kneewidthmax, onemeterdistance, rightkneeanglemin,
                                            rightkneeangleavg, rightkneeanglemax, rightankleanglemin, rightankleangleavg,
                                            rightankleanglemax, righthipanglemin, righthipangleavg,
                                            righthipanglemax, leftkneeanglemin, leftkneeangleavg, leftkneeanglemax,
                                            leftankleanglemin, leftankleangleavg, leftankleanglemax,
                                            lefthipanglemin, lefthipangleavg, lefthipanglemax, rightarmanglemin,
                                            rightarmangleavg, rightarmanglemax, leftarmanglemin,
                                            leftarmangleavg, leftarmanglemax, backanglemin, backangleavg, backanglemax]])      
                                                                                                                                                   
     if predict == 0:                                                                                    
         prediction_text='Healthy'
     elif predict == 1:
         prediction_text="Disease Found: Stroke, please look the details from Diseases Page."
     elif predict == 2:
         prediction_text="Disease Found: Parkinson's, please look the details from Diseases Page."
     elif predict == 3:
         prediction_text="Disease Found: Cerebellar Ataxia, please look the details from Diseases Page."
     elif predict == 4:
          prediction_text="Disease Found: Toe Walking, please look the details from Diseases Page."
     elif predict == 5:
         prediction_text='Disease Found: Bowleg, please look the details from Diseases Page.'
     else:
          return render_template('index.html')
    
     hastaliklar = open("hastaliklar.txt", "w")         #writing predict result to txt
     hastaliklar.write(prediction_text)
     hastaliklar.close()
    
      
    return render_template("index.html")



@app.route('/about')            
def about():                                                        #sends data to about.html
   return render_template("about.html")


@app.route('/sample')           
def sample():                                                       #sends data to sample.html
   return render_template("sample.html")


@app.route('/predict')                     
def predict():                                                       #gets predict result from txt, posts prediction                        
      with open("hastaliklar.txt","r") as file:                      
       hastaliklar = file.readlines()
      return render_template("index.html", hastaliklar=hastaliklar)
 

@app.route("/feedback", methods=["GET", "POST"])       
def feedback():                                                      #posts feedback
  if request.method == "GET":
    return render_template("feedback.html")  
  elif request.method == "POST":
    userdata = dict(request.form)
    sentence = userdata["sentence"]
    a=sentence
    feed = userdata["feed"]
    with open('feedback.csv', mode='a') as csv_file:
      data = csv.writer(csv_file, delimiter=';', quotechar='"', quoting=csv.QUOTE_MINIMAL)
      data.writerow([sentence, feed])                               #dataset is updated
  return render_template('feedback.html',text="Thank you!",text1=a)

if __name__ == '__main__':
   app.run(debug=True, threaded=False)
