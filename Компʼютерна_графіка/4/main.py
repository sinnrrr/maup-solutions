from tkinter import *
from tkinter import filedialog

import cv2
import numpy as np


def apply_filter(image, filter_type):
    if filter_type == "Чорно-білий":
        return cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    elif filter_type == "Сепія":
        kernel = np.array(
            [[0.393, 0.769, 0.189], [0.349, 0.686, 0.168], [0.272, 0.534, 0.131]]
        )
        return cv2.filter2D(image, -1, kernel)
    elif filter_type == "Негатив":
        return cv2.bitwise_not(image)
    else:
        return image


def open_image():
    file_path = filedialog.askopenfilename()
    image = cv2.imread(file_path)
    cv2.imshow("Original Image", image)

    filter_type = filter_var.get()
    filtered_image = apply_filter(image, filter_type)

    cv2.imshow("Filtered Image", filtered_image)
    cv2.waitKey(0)
    cv2.destroyAllWindows()


# Створення графічного інтерфейсу Tkinter
root = Tk()
root.title("Накладання фільтрів на зображення")

# Вибір фільтра
filter_var = StringVar(root)
filter_var.set("Чорно-білий")
filter_options = OptionMenu(root, filter_var, "Чорно-білий", "Сепія", "Негатив")
filter_options.pack()

# Кнопка відкриття зображення
open_button = Button(root, text="Відкрити зображення", command=open_image)
open_button.pack()

root.mainloop()
