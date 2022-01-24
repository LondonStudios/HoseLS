fx_version 'bodacious'
games { 'gta5' }

author 'London Studios & Adam Fenton / Converted to lua by Dr0msis#2562 and Pingouin#8843'
description 'A resource providing a realistic firefighting experience, featuring a water hose.'

client_scripts {
    'client.lua',
}

server_scripts {
    'server.lua',
}

files {
    'hose/contentunlocks.meta',
	'hose/loadouts.meta',
	'hose/pedpersonality.meta',
	'hose/shop_weapon.meta',
	'hose/weaponanimations.meta',
	'hose/weaponarchetypes.meta',
	'hose/weapons.meta',
}

data_file 'WEAPONINFO_FILE' 'hose/weapons.meta'
data_file 'WEAPON_METADATA_FILE' 'hose/weaponarchetypes.meta'
data_file 'WEAPON_SHOP_INFO' 'hose/shop_weapon.meta'
data_file 'WEAPON_ANIMATIONS_FILE' 'hose/weaponanimations.meta'
data_file 'CONTENT_UNLOCKING_META_FILE' 'hose/contentunlocks.meta'
data_file 'LOADOUTS_FILE' 'hose/loadouts.meta'
data_file 'PED_PERSONALITY_FILE' 'hose/pedpersonality.meta'