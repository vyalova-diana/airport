from flask import Flask
from flask import request
import random
import logging

FORMAT = '%(asctime)-15s %(message)s'
logging.basicConfig(format=FORMAT)
logger = logging.getLogger('tcpserver')


class Car:
    def __init__(self, ID):
        self.ID = ID
        self.status = "free"
        self.plainID = None
        self.cargos = "{}"


cars = [Car(1), Car(2), Car(3)]
app = Flask(__name__)


def getFreeCar():
    for car in cars:
        if car.status == "free":
            return car
    raise Exception("No free car")

def getCarBy(ID):
    for car in cars:
        if car.ID == ID
            return car
    raise Exception("No car with such ID")

def carArrivedWaiter():

@app.route('/car/<CAR_ID>/sendToAirplane/<PLAIN_ID>', methods=["POST"])
def sendToAirplane(PLAIN_ID):
    if CAR_ID is None:
        try:
            car = getFreeCar()
            car.plainID = PLAIN_ID
            logger.warning('Success: %s', 'Car {} sent to plane {}'.format(car.ID, car.plainID))
            return '{ ID: }'
        except Exception as Error:
            logger.warning('Error: %s', 'No free')
            return '{ error: 1, description: "No free car"}'
    else
        try:
            car = getCarBy(CAR_ID)
            car.plainID = PLAIN_ID
            logger.warning('Success: %s', 'Car {} sent to plane {}'.format(car.ID, car.plainID))
            return '{ ID: }'
        except:
            logger.warning('Error: %s', 'No car with ID: {}'.format(CAR_ID))
            return '{ error: 1, description: "No free car"}'


@app.route('/car/<CAR_ID>/cargos', methods=['GET', 'POST'])
def cargos(CAR_ID):
    if request.method == 'POST':
        try:
            car = getCarBy(CAR_ID)
            json = request.get_json()
            car.cargos = json
            car.status = "ready_to_go"
            logger.warning('Success: %s', 'Put cargos: {} in the car with ID: '.format(json, CAR_ID))
            return ""
        except:
            logger.warning('Error: %s', 'No car with ID: {}'.format(CAR_ID))
            return '{ error: 1, description: "No free car"}'
    else:
        try:
            car = getCarBy(CAR_ID)
            cargos = car.cargos
            car.status = "ready_to_go"
            car.cargos = "{}"
            logger.warning('Success: %s', 'Get cargos: {} from the car with ID: '.format(json, CAR_ID))
            return cargos
        except:
            logger.warning('Error: %s', 'No car with ID: {}'.format(CAR_ID))
            return '{ error: 2, description: "No car with such ID"}'


@app.route('/car/<id>/sendToStorage', methods=["POST"])
def sendToStorage(id):
    try:
        car = getCarBy(CAR_ID)
        car.status = "way_to_storage"
        logger.warning('Success: %s', 'Put cargos: {} in the car with ID: '.format(json, CAR_ID))
        return '{ ID: {}}'.format(id)
    except Exception as Error:
        logger.warning('Error: %s', 'No car with ID: {}'.format(CAR_ID))
        return '{ error: 2, description: "No car with such ID"}'
