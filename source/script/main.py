import requests
import subprocess
from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from webdriver_manager.chrome import ChromeDriverManager
import random
import os
from PIL import Image
import ctypes
import sys
import json
import time

release = requests.get('https://raw.githubusercontent.com/rikumomo0407/Wallpaper-Searcher/main/update/versions.txt').content.decode('cp932').split('\n')
with open("update/version.log", 'r', encoding="UTF-8") as f:
    version = f.read().split('\n')
if release[1] != version[1]:
    subprocess.Popen("WallpaperSearcherUpdate.exe")
    sys.exit()

cwd = os.getcwd()
with open("materials/data.json", 'r', encoding="UTF-8") as f:
    data = json.load(f)
with open("images/stock/detail.txt", 'r', encoding="UTF-8") as f:
    stock_detail = f.readlines()
with open("images/detail.txt", 'w', encoding="UTF-8") as f:
    f.writelines(stock_detail)

if data["setting"]["visible"] == False:
    options = webdriver.ChromeOptions()
    options.add_argument("--headless")
    driver = webdriver.Chrome(ChromeDriverManager().install(), options=options)
else:
    driver = webdriver.Chrome(ChromeDriverManager().install())

for _ in range(3):
    try:
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
        with open("materials/data.json", 'w', encoding='UTF-8') as f:
            json.dump(data, f, indent=2)
    except Exception as e:
        pass
    else:
        break

for _ in range(3):
    try:
        img = Image.open("images/stock/wallpaper.png")
        img.save("images/wallpaper.png")
        ctypes.windll.user32.SystemParametersInfoW(20, 0, cwd+"\\images\\wallpaper.png", 0)
        driver.get(data["words"][random.randrange(len(data["words"]))][1])
        win_height = driver.execute_script("return window.innerHeight")
        last_top = 1
        while True:
            last_height = driver.execute_script("return document.body.scrollHeight")
            top = last_top
            while top < last_height:
                top += int(win_height * 0.8)
                driver.execute_script("window.scrollTo(0, %d)" % top)
                time.sleep(0.3)
                new_last_height = driver.execute_script("return document.body.scrollHeight")
            if last_height == new_last_height:
                web_range = len(driver.find_elements("xpath", "/html/body/div[2]/c-wiz/div[3]/div[1]/div/div/div/div/div[1]/div[1]/span/div[1]/div[1]/div"))
                if data["setting"]["range"] >= web_range:
                    loadcheck = WebDriverWait(driver, 15).until(lambda x: x.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[1]/div/div/div/div/div[1]/div[2]/div[2]"))
                    if loadcheck.get_attribute("style") == "":
                        loadcheck.click()
                    else:
                        break
                else:
                    break
            last_top = last_height
        time.sleep(1)
        while True:
            img_num = random.randrange(web_range) + 1 if data["setting"]["range"] >= web_range else random.randrange(data["setting"]["range"]) + 1
            driver_result = WebDriverWait(driver, 15).until(lambda x: x.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[1]/div/div/div/div/div[1]/div[1]/span/div[1]/div[1]/div[" + str(img_num) + "]/a[1]/div[1]/img"))
            driver_result.click()
            url_detail = WebDriverWait(driver, 15).until(lambda x: x.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[2]/div[3]/div[2]/div/div[2]/div[2]/div[2]/c-wiz/div/div[1]/div[2]/div[2]/div/a").get_attribute("href"))
            url = WebDriverWait(driver, 15).until(lambda x: x.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[2]/div[3]/div[2]/div/div[2]/div[2]/div[2]/c-wiz/div/div[1]/div[2]/div[2]/div/a/img").get_attribute("src"))
            if url[0:3] != "data":
                for i in data["rejected"]:
                    if i == url:
                        break
                else:
                    data["rejected"][1] = data["rejected"][0]
                    data["rejected"][0] = url
                    with open("materials/data.json", 'w', encoding="UTF-8") as f:
                        json.dump(data, f, indent=2)
                    if url[0:7] == "https:/":
                        r = requests.get(url)
                        with open("images/stock/wallpaper.png","wb") as f:
                            f.write(r.content)
                        with open("images/stock/detail.txt",'w') as f:
                            f.write(url_detail)
                        im = Image.open("images/stock/wallpaper.png")
                        if ((data["setting"]["vertical"] != 0 and im.width != data["setting"]["vertical"]) or (data["setting"]["horizontal"] != 0 and im.height != data["setting"]["horizontal"]) or (data["setting"]["min_vertical"] != 0 and im.width >= data["setting"]["min_vertical"]) or (data["setting"]["min_vertical"] != 0 and im.height >= data["setting"]["min?horizontal"])) == False:
                            driver.quit()
                            break
    except Exception as e:
        pass
    else:
        break
else:
    driver.quit()
    from win10toast import ToastNotifier
    toast = ToastNotifier()
    toast.show_toast("壁紙の変更に失敗しました", "ドライバエラー", duration = 10, icon_path = "materials/app.ico")
sys.exit()
