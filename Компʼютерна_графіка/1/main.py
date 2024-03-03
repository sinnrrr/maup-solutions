import matplotlib.pyplot as plt
import numpy as np


def mandelbrot(c, max_iter):
    z = 0
    n = 0
    while abs(z) <= 2 and n < max_iter:
        z = z**2 + c
        n += 1
    return n


def mandelbrot_set(xmin, xmax, ymin, ymax, width, height, max_iter):
    r1 = np.linspace(xmin, xmax, width)
    r2 = np.linspace(ymin, ymax, height)
    return (
        r1,
        r2,
        np.array([[mandelbrot(complex(r, i), max_iter) for r in r1] for i in r2]),
    )


def display(xmin, xmax, ymin, ymax, width, height, max_iter):
    d = mandelbrot_set(xmin, xmax, ymin, ymax, width, height, max_iter)
    plt.imshow(d[2], extent=(xmin, xmax, ymin, ymax))
    plt.show()


# Відображення Множини Мандельброта для певних параметрів
display(-2.0, 1.0, -1.5, 1.5, 1000, 1000, 256)
