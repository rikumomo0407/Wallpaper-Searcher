from selenium import webdriver
from webdriver_manager.chrome import ChromeDriverManager
import os
import pyautogui as pg
import json

cwd = os.getcwd()
width, height = pg.size()
with open("script/data.json", 'r', encoding='UTF-8') as f:
    data = json.load(f)

options = webdriver.ChromeOptions()
options.add_argument('--headless')
driver = webdriver.Chrome(ChromeDriverManager().install(), options=options)
driver.set_window_size(width,height)

for word in data["words"]:
    if word[1] == "":
        driver.get('https://www.google.co.jp')
        search_bar = driver.find_element("name", "q")
        search_bar.send_keys(word[0])
        search_bar.submit()
        for elem in driver.find_elements("xpath", '/html/body/div[7]/div/div[4]/div/div[1]/div[1]/div[1]/div'):
            strip_index = 1
            for j in elem.text:
                if j == '像':
                    break
                if j == '\n':
                    strip_index += 1
            for elem in driver.find_elements("xpath", '/html/body/div[7]/div/div[4]/div/div[1]/div[1]/div[1]/div/div[' + str(strip_index) + ']/a'):
                word[1] = elem.get_attribute("href")

with open("script/data.json", 'w', encoding='UTF-8') as f:
    json.dump(data, f, indent=2)

driver.quit()