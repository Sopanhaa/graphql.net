"""Check the live gateway and its gRPC integration using only Python's standard library."""
import argparse
import json
import ssl
import urllib.request

parser = argparse.ArgumentParser()
parser.add_argument("--url", default="https://localhost:7002/graphql")
parser.add_argument("--ca-file")
args = parser.parse_args()
context = ssl.create_default_context(cafile=args.ca_file)

def query(document):
    request = urllib.request.Request(args.url,
        data=json.dumps({"query": document}).encode(),
        headers={"Content-Type": "application/json"})
    with urllib.request.urlopen(request, context=context, timeout=15) as response:
        result = json.load(response)
    assert "errors" not in result, result
    return result["data"]

products = query("{ products { id name description price category stockQuantity } }")["products"]
assert len(products) == 8
assert {p["category"] for p in products} == {"Electronics", "Books", "Home"}
assert all(set(p) == {"id", "name", "description", "price", "category", "stockQuantity"} for p in products)
selected = query("{ products { id name price } }")["products"]
assert all(set(p) == {"id", "name", "price"} for p in selected)
assert query("{ productById(id: 1) { id name } }")["productById"] == {"id": 1, "name": "Wireless Mouse"}
assert query("{ productById(id: 999) { id } }")["productById"] is None
books = query('{ products(category: "books") { id category } }')["products"]
assert len(books) == 3 and all(p["category"] == "Books" for p in books)
assert query('{ products(category: "Unknown") { id } }')["products"] == []
print("PASS: all products, field selection, existing ID, missing ID, case-insensitive filter, empty filter result")
