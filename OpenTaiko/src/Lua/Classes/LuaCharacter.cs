using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FDK;

namespace OpenTaiko {
	public class LuaCharacter {
		public string FolderName { get; private set; } = "";

		public Dictionary<string, Dictionary<string, LuaTexture[]>> Sprites { get; private set; } = new();
		public Dictionary<string, Dictionary<string, LuaSound>> Sounds { get; private set; } = new();

		public LuaCharacter(string folder_name, bool legacy = false) {
			FolderName = folder_name;
		}

		public void LoadCharacter() {
			#region Initialize
			#region Bulk Loading Methods
			LuaTexture[] loadTextures(string file_path) {
				int count = OpenTaiko.t連番画像の枚数を数える(file_path + Path.DirectorySeparatorChar);
				LuaTexture[] textures = new LuaTexture[count];

				for (int i = 0; i < count; i++) {
					textures[i] = new(new CTexture(file_path + Path.DirectorySeparatorChar + $"{i}.png", false));
				}
				return textures;
			}

			Dictionary<string, LuaSound> loadSounds(string file_path) {
				Dictionary<string, LuaSound> sounds = [];
				string[] files = Directory.GetFiles(file_path)
					.Where(item => item.ToLower().EndsWith(".ogg") || item.ToLower().EndsWith(".wav"))
					.ToArray();

				foreach (string file in files) {
					string name = Path.GetFileNameWithoutExtension(file);
					if (sounds.ContainsKey(name)) continue;
					sounds.Add(name, new(file, ESoundGroup.Voice));
				}

				return sounds;
			}
			#endregion

			// Textures
			string path = OpenTaiko.strEXEのあるフォルダ + TextureLoader.GLOBAL + TextureLoader.CHARACTERS + FolderName;
			string[] directories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly).Where(item => !item.EndsWith("Sounds")).ToArray();

			// Sounds
			string sound_path = Path.Combine(path, "Sounds");
			string[] sound_dirs = Directory.GetDirectories(sound_path, "*", SearchOption.TopDirectoryOnly);
			#endregion

			// Textures
			Sprites.Add("", new());
			foreach (string dir in directories) {

				int sprite_count = OpenTaiko.t連番画像の枚数を数える(dir + Path.DirectorySeparatorChar);
				string[] subs = Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly);
				string name = Path.GetRelativePath(path, dir);

				if (sprite_count > 0) {
					Sprites[""].Add(name, loadTextures(dir));
				}
				else if (subs.Length > 0) {
					Sprites.Add(name, new()); // New subcategory

					foreach (string sub in subs) {
						string subname = Path.GetRelativePath(dir, sub);
						Sprites[name].Add(subname, loadTextures(sub));
					}
				}
				else {
					Sprites[""].Add(name, []);
				}
			}

			// Sounds
			Sounds.Add("", loadSounds(sound_path));
			foreach (string dir in sound_dirs) {
				string name = Path.GetRelativePath(sound_path, dir);
				Sounds.Add(name, loadSounds(dir));
			}

			int test = 0; // ez breakpoint
		}
	}
}
