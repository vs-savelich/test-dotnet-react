import localforage from "localforage";

const itemName = "tenants";

export type Tenant = {
    id: string;
    name: string;
    country: string;
};

export async function getTenants() {
    let tenants = await localforage.getItem<Tenant[]>(itemName);
    if (!tenants) {
        const response = await fetch("tenant");
        tenants = response.ok ? await response.json() : [];
    }
    return tenants ?? [];
}

export async function createTenant(tenant: Tenant) {
    const response = await fetch("tenant", { method: "POST", body: JSON.stringify(tenant) });
    tenant = await response.json();
    const tenants = await getTenants();
    tenants.unshift(tenant);
    await set(tenants);
    return tenant;
}

export async function updateTenant(tenant: Tenant) {
    await fetch("tenant", { method: "PUT", body: JSON.stringify(tenant) });
    const tenants = await getTenants();
    const existing = tenants.find(t => t.id === tenant.id)!;
    existing.name = tenant.name;
    existing.country = tenant.country;
    await set(tenants);
    return tenant;
}

export async function getTenant(id: string) {
    const tenants = await localforage.getItem<Tenant[]>(itemName) ?? [];
    return tenants.find(t => t.id === id);
}

export async function deleteTenant(id: string) {
    await fetch(`tenant/${id}`, { method: "DELETE" });
    const tenants = await localforage.getItem<Tenant[]>(itemName) ?? [];
    const index = tenants.findIndex(t => t.id === id);
    if (index > -1) {
        tenants.splice(index, 1);
        await set(tenants);
        return true;
    }
    return false;
}

function set(tenants: Tenant[]) {
    return localforage.setItem(itemName, tenants);
}