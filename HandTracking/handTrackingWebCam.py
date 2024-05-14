import cv2
from cvzone.HandTrackingModule import HandDetector
import socket

cap = cv2.VideoCapture(0)
detector = HandDetector(maxHands= 2,detectionCon=0.8)

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddress = ("127.0.0.1", 8080)
width, height = 640, 480
cap.set(3, width)
cap.set(4, height)

while True:
    success, img = cap.read()
    if success == False:
        break
    hands, img = detector.findHands(img)
    data = []
    if hands:
        hand = hands[0]
        lmList = hand["lmList"]
        cv2.circle(img, (lmList[0][:2]), 2, (255, 255, 255), thickness=2, lineType=None, shift=None)
        for lm in lmList:
            data.extend([lm[0], height - lm[1], lm[2]])
        sock.sendto(str.encode(str(data)), serverAddress)
    

    cv2.imshow("Image", img)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cv2.destroyAllWindows()
cap.release()