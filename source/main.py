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
    from plyer import notification
    notification.notify(title="壁紙の変更に失敗しました", message=error_message, app_name="Wallpaper-Searcher", app_icon="icon.ico", timeout=10)
    driver.quit()
    sys.exit()

cwd = os.getcwd()
with open('data.json', 'r', encoding='UTF-8') as f:
    data = json.load(f)

with open('images/stock/detail.txt', 'r', encoding='UTF-8') as f:
    stock_detail = f.readlines()
img = Image.open('images/stock/wallpaper.png')
imgs = Image.open('images/stock/wallpapers.png')
with open('images/detail.txt', 'w', encoding='UTF-8') as f:
    f.writelines(stock_detail)
img.save('images/wallpaper.png')
imgs.save('images/wallpapers.png')

ctypes.windll.user32.SystemParametersInfoW(20, 0, cwd+'\images\wallpapers.png', 0)

monitor_info = GetMonitorInfo(MonitorFromPoint((0,0)))
monitor_area = monitor_info.get("Monitor")
width = monitor_area[2]
height = monitor_area[3]
work_area = monitor_info.get("Work")
rheight = work_area[3]

try:
    if data['setting']['visible'] == 0:
        options = webdriver.ChromeOptions()
        options.add_argument('--headless')
        driver = webdriver.Chrome(ChromeDriverManager().install(), options=options)
    else:
        driver = webdriver.Chrome(ChromeDriverManager().install())
except:
    Error("Error:S001(ドライバエラー)")
    
driver.set_window_size(width,height)

roop_num = 0
research = True
while(research):
    img_num = random.randrange(400) + 1
    elem = 0
    research = False
    roop_num += 1
    try:
        driver.get(data['search_url'][random.randrange(len(data['search_url']))].rstrip('\n'))
    except:
        Error("Error:U001(インターネットエラー)")
    for i in range(5):
        driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
        time.sleep(1)
    time.sleep(3)
    for elem in driver.find_elements("xpath", "/html/body/div[2]/c-wiz/div[3]/div[1]/div/div/div/div/div[1]/div[1]/span/div[1]/div[1]/div[" + str(img_num) + "]/a[1]/div[1]/img"):
        if elem != 0:
            driver.execute_script("arguments[0].click();", elem)
            time.sleep(5)
            url_detail = driver.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[2]/div[3]/div[2]/div/div[2]/div[2]/div[2]/c-wiz/div[2]/div[1]/div[1]/div[2]/div/a").get_attribute('href')
            url = driver.find_element("xpath", "/html/body/div[2]/c-wiz/div[3]/div[2]/div[3]/div[2]/div/div[2]/div[2]/div[2]/c-wiz/div[2]/div[1]/div[1]/div[2]/div/a/img").get_attribute('src')
            if url[0:3] != 'data':
                for i in data['rejected-url']:
                    if url == i.rstrip('\n'):
                        research = True
                data['rejected-url'][0] = url
                with open('data.json', 'w', encoding='UTF-8') as f:
                    json.dump(data, f, indent=4)
                HTTP_HEADERS = {'User-Agent':'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.41 Safari/537.36'}
                if url[0:7] == 'https:/':
                    r = requests.get(url, headers=HTTP_HEADERS)
                    with open('images/stock/wallpaper.png','wb') as f:
                        f.write(r.content)
                im = Image.open('images/stock/wallpaper.png')
                if data['setting']['width'] != 0 or data['setting']['height'] != 0:
                    if data['setting']['height'] == 0 and im.width != data['setting']['width']:
                        research = True
                    if data['setting']['width'] == 0 and im.height != data['setting']['height']:
                        research = True
    if roop_num > 3:
        Error("Error:S002(ドライバエラー)")

driver.quit()

if data['setting']['width'] == 1:
    ratio = min(width / im.width, rheight / im.height)
else:
    ratio = max(width / im.width, rheight / im.height)
resize_size = (round(ratio * im.width), round(ratio * im.height))
resized_image = im.resize(resize_size)
result = Image.new("RGBA", (width,height), (data['setting']['background-color'][0], data['setting']['background-color'][1], data['setting']['background-color'][2]))
if ratio == width / im.width:
    result.paste(resized_image, (0, round((rheight-(ratio * im.height))/2)))
else:
    result.paste(resized_image, (round((width-(ratio * im.width))/2), 0))
result.save('images/stock/wallpapers.png', quality=95)

with open('images/stock/detail.txt','w') as f:
    f.write(url_detail)

sys.exit()
