from selenium import webdriver
from webdriver_manager.chrome import ChromeDriverManager
import random
import os
import requests
from PIL import Image
import ctypes
from win32api import GetMonitorInfo, MonitorFromPoint
import sys
import json
import time

def Error(error_message):
    driver.quit()
    from plyer import notification
    notification.notify(title="壁紙の変更に失敗しました", message=error_message, app_name="Wallpaper Searcher", app_icon="app.ico", timeout=10)
    sys.exit()

cwd = os.getcwd()

with open("script/data.json", 'r', encoding="UTF-8") as f:
    data = json.load(f)

with open("script/images/stock/detail.txt", 'r', encoding="UTF-8") as f:
    stock_detail = f.readlines()
with open("script/images/detail.txt", 'w', encoding="UTF-8") as f:
    f.writelines(stock_detail)
img = Image.open("script/images/stock/wallpaper.png")
img.save("script/images/wallpaper.png")

ctypes.windll.user32.SystemParametersInfoW(20, 0, cwd+"\\script\\images\\wallpaper.png", 0)

monitor_info = GetMonitorInfo(MonitorFromPoint((0,0)))
monitor_area = monitor_info.get("Monitor")
width = monitor_area[2]
height = monitor_area[3]

try:
    if data["setting"]["visible"] == 0:
        options = webdriver.ChromeOptions()
        options.add_argument("--headless")
        driver = webdriver.Chrome(ChromeDriverManager().install(), options=options)
    else:
        driver = webdriver.Chrome(ChromeDriverManager().install())
except Exception as e:
    Error("Error:S001(ドライバエラー)")
    
driver.set_window_size(width,height)

roop_num = 0
research = True
while(research):
    elem = 0
    research = False
    try:
        driver.get(data["words"][random.randrange(len(data["words"]))][1])
    except Exception as e:
        Error("Error:U002(インターネットエラー)")
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
                time.sleep(1)
                loadcheck = driver.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[1]/div/div/div/div/div[1]/div[2]/div[2]")
                if loadcheck.get_attribute("style") == "":
                    loadcheck.click()
                else:
                    break
            else:
                break
        last_top = last_height
    time.sleep(1)
    try:
        clickloop = True
        while clickloop:
            try:
                img_num = random.randrange(web_range) + 1 if data["setting"]["range"] >= web_range else random.randrange(data["setting"]["range"]) + 1
                print(img_num)
                driver_result = driver.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[1]/div/div/div/div/div[1]/div[1]/span/div[1]/div[1]/div[" + str(img_num) + "]/a[1]/div[1]/img")
                time.sleep(1)
                driver_result.click()
                clickloop = False
            except Exception as e:
                pass
        time.sleep(1.5)
        url_detail = driver.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[2]/div[3]/div[2]/div/div[2]/div[2]/div[2]/c-wiz/div[2]/div[1]/div[1]/div[2]/div/a").get_attribute("href")
        url = driver.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[2]/div[3]/div[2]/div/div[2]/div[2]/div[2]/c-wiz/div[2]/div[1]/div[1]/div[2]/div/a/img").get_attribute("src")
        if url[0:3] != "data":
            for i in data["rejected"]:
                if i == url:
                    research = True
            data["rejected"][1] = data["rejected"][0]
            data["rejected"][0] = url
            with open("data.json", 'w', encoding="UTF-8") as f:
                json.dump(data, f, indent=2)
            HTTP_HEADERS = {"User-Agent":"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.41 Safari/537.36"}
            if url[0:7] == "https:/":
                r = requests.get(url, headers=HTTP_HEADERS)
                with open("script/images/stock/wallpaper.png","wb") as f:
                    f.write(r.content)
                with open("script/images/stock/detail.txt",'w') as f:
                    f.write(url_detail)
            im = Image.open("script/images/stock/wallpaper.png")
            if data["setting"]["vertical"] != 0 and im.width != data["setting"]["vertical"]:
                research = True
            if data["setting"]["horizontal"] != 0 and im.height != data["setting"]["horizontal"]:
                research = True
            if data["setting"]["min_vertical"] != 0 and im.width >= data["setting"]["min_vertical"]:
                research = True
            if data["setting"]["min_vertical"] != 0 and im.height >= data["setting"]["min?horizontal"]:
                research = True
        else:
            research = True
    except Exception as e:
        research = True
    if roop_num > 1:
        Error("Error:S002(ドライバエラー)")
    else:
        roop_num += 1

driver.quit()
sys.exit()