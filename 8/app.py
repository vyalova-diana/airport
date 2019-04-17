from flask import Flask
app = Flask(__name__)

# Availiable status for the plane:
# - "arrived"
# - "waiting_follow"
# - ""
class Plane:
    def __init__(self, ID):
        self.ID = ID
        self.status = "arrived"

def ask_follow_me():
    return ""
