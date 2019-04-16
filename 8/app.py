from flask import Flask
app = Flask(__name__)

@app.route("/car")
def hello():
    return "Hello World!"

@app.route("/car/*ID*/sendToStorage", methods=['POST'])
def hello():
    return "Hello World!"

@app.route("/car/*ID1*/sendToAirplane/*ID2*", methods=['POST'])
def hello():
    return "Hello World!"

@app.route("/cargos", methods=['POST', 'GET'])
def hello():
    return "Hello World!"
