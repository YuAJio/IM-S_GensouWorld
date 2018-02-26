using System;
using System.Collections.Generic;
using System.IO;
using Baidu.Aip.ImageCensor;
using Newtonsoft.Json;

namespace Baidu.Aip.Demo
{
    internal class ImageSearchDemo
    {
        ImageSearch.ImageSearch client = new ImageSearch.ImageSearch("apiKey", "secretKey");
        
        public void SameHqAddDemo() {
            var image = File.ReadAllBytes("图片文件路径");
            // 调用相同图检索—入库
            var result = client.SameHqAdd(image);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"brief", "{\"name\":\"周杰伦\", \"id\":\"666\"}" } 
            };
            // 带参数调用相同图检索—入库
            result = client.SameHqAdd(image, options);
            Console.WriteLine(result);
        }
        
        public void SameHqSearchDemo() {
            var image = File.ReadAllBytes("图片文件路径");
            // 调用相同图检索—检索
            var result = client.SameHqSearch(image);
            Console.WriteLine(result);
        }
        
        public void SameHqDeleteByImageDemo() {
            var image = File.ReadAllBytes("图片文件路径");
            // 调用删除相同图，传入参数为图片
            var result = client.SameHqDeleteByImage(image);
            Console.WriteLine(result);
        }
        public void SameHqDeleteBySignDemo() {
            var contSign = "8cnn32frvrr2cd901";
	
            // 调用删除相同图，传入参数为图片签名
            var result = client.SameHqDeleteBySign(contSign);
            Console.WriteLine(result);
        }
        public void SimilarAddDemo() {
            var image = File.ReadAllBytes("图片文件路径");
            // 调用相似图检索—入库
            var result = client.SimilarAdd(image);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                {"brief", "{\"name\":\"周杰伦\", \"id\":\"666\"}" } 
            };
            // 带参数调用相似图检索—入库
            result = client.SimilarAdd(image, options);
            Console.WriteLine(result);
        }
        
        public void SimilarSearchDemo() {
            var image = File.ReadAllBytes("图片文件路径");
            // 调用相似图检索—检索
            var result = client.SimilarSearch(image);
            Console.WriteLine(result);
        }
        
        public void SimilarDeleteByImageDemo() {
            var image = File.ReadAllBytes("图片文件路径");
            // 调用删除相似图，传入参数为图片
            var result = client.SimilarDeleteByImage(image);
            Console.WriteLine(result);
        }
        public void SimilarDeleteBySignDemo() {
            var contSign = "8cnn32frvrr2cd901";
	
            // 调用删除相似图，传入参数为图片签名
            var result = client.SimilarDeleteBySign(contSign);
            Console.WriteLine(result);
        }
    }
    
    
}